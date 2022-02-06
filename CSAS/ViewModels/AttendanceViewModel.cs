using System.Globalization;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
	public class AttendanceViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand SelectLectureCommand { get; }
		public DelegateCommand<string?> SelectSeminarCommand { get; }
		public DelegateCommand<string?> SelectAttendanceCommand { get; }
		public DelegateCommand AddNewAttendanceCommand { get; }
		public DelegateCommand<string?> RemoveAttendanceCommand { get; }
		public DelegateCommand<object?> StudentPresentCommand { get; }

		private ObservableCollection<Attendance> _lectures;
		public ObservableCollection<Attendance> Lectures
		{
			get { return _lectures; }
			set
			{
				SetProperty(ref _lectures, value);
			}
		}

		private ObservableCollection<Attendance> _seminars;
		public ObservableCollection<Attendance> Seminars
		{
			get { return _seminars; }
			set
			{
				SetProperty(ref _seminars, value);
			}
		}

		private Attendance _selectedAttendance;
		public Attendance SelectedAttendance
		{
			get { return _selectedAttendance; }
			set
			{
				SetProperty(ref _selectedAttendance, value);
			}
		}

		private ObservableCollection<SubGroup> _subGroups;
		public ObservableCollection<SubGroup> SubGroups
		{
			get { return _subGroups; }
			set
			{
				SetProperty(ref _subGroups, value);
			}
		}

		private SubGroup _selectedSubGroup;

		public SubGroup SelectedSubGroup
		{
			get { return _selectedSubGroup; }
			set
			{
				SetProperty(ref _selectedSubGroup, value);
			}
		}
		private ObservableCollection<Attendance> _attendances;
		public ObservableCollection<Attendance> Attendances
		{
			get { return _attendances; }
			set
			{
				SetProperty(ref _attendances, value);
			}
		}

		private bool _isLectureSelected = true;
		public bool IsLectureSelected
		{
			get { return _isLectureSelected; }
			set
			{
				SetProperty(ref _isLectureSelected, value);
			}
		}

		private bool _isAttendanceSelected;
		public bool IsAttendanceSelected
		{
			get { return _isAttendanceSelected; }
			set
			{
				SetProperty(ref _isAttendanceSelected, value);
			}
		}

		private string _numberOfStudents;
		public string NumberOfStudents
		{
			get { return _numberOfStudents; }
			set
			{
				SetProperty(ref _numberOfStudents, value);
			}
		}
		private new UnitOfWork Work { get; set; }

		private DateTime _time = DateTime.Now;
		public DateTime Time
		{
			get { return _time; }
			set
			{
				SetProperty(ref _time, value);
			}
		}
		public AttendanceViewModel(string currentMainGroupId)
		{
			Work = UoWSingleton.Instance;
			Attendances = new ObservableCollection<Attendance>();

			var att = Work.Attendance.GetAttendanceByMainGroup(Work.MainGroup.Get(currentMainGroupId));
			Lectures = new ObservableCollection<Attendance>(att.Where(x => x.Form == AttendanceFormEnums.Lecture));
			Seminars = new ObservableCollection<Attendance>(att.Where(x => x.Form == AttendanceFormEnums.Seminar));
			Attendances = Lectures;
			SubGroups = new ObservableCollection<SubGroup>(Work.MainGroup.Get(currentMainGroupId).SubGroups);
			SelectedSubGroup = SubGroups.FirstOrDefault();
			SelectLectureCommand = new DelegateCommand(SelectLecture);
			SelectSeminarCommand = new DelegateCommand<string?>(SelectSeminar);
			AddNewAttendanceCommand = new DelegateCommand(AddNewAttendance);
			RemoveAttendanceCommand = new DelegateCommand<string?>(RemoveAttendance);
			SelectAttendanceCommand = new DelegateCommand<string?>(SelectAttendance);
			StudentPresentCommand = new DelegateCommand<object?>(StudentPresent);

		}

		private void SelectAttendance(string? id)
		{

			if (!string.IsNullOrEmpty(id))
			{
				IsAttendanceSelected = true;

				SelectedAttendance = Work.Attendance.Get(id);
			}
			else
			{
				IsAttendanceSelected = false;
			}

		}

		private void SelectLecture()
		{
			Attendances = new ObservableCollection<Attendance>(Lectures.OrderByDescending(x => x.Date.Date).ToList());
			IsLectureSelected = true;
		}
		private void SelectSeminar(string? id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				SelectedSubGroup = Work.SubGroup.Get(id);
				Attendances = new ObservableCollection<Attendance>(Work.Attendance.GetAttendanceBySubGroup(SelectedSubGroup).OrderByDescending(x => x.Date));
			}
			else if (SelectedSubGroup != null)
			{
				Attendances = new ObservableCollection<Attendance>(Work.Attendance.GetAttendanceBySubGroup(SelectedSubGroup).OrderByDescending(x => x.Date));
			}
			else
			{
				Attendances = new ObservableCollection<Attendance>();
			}

			IsLectureSelected = false;
		}
		private void ResetAttendance()
		{
			var temp = SelectedAttendance;

			SelectedAttendance = new Attendance();
			SelectedAttendance = temp;
		}
		private async void StudentPresent(object? obj)
		{
			IsLoading = true;
			await System.Threading.Tasks.Task.Run(() =>
			{
				var param = obj as object[];
				var en = GetType((string)param[1]);
				if (!SelectedAttendance.SubAttendances.Where(x => x.IsSelected).Any())
				{
					var att = SelectedAttendance.SubAttendances.FirstOrDefault(x => x.Id == (string)param[0]);
					att.State = en;
					Work.Attendance.Update(SelectedAttendance);
				}
				else
				{
					foreach (var subAtt in SelectedAttendance.SubAttendances.Where(x => x.IsSelected))
					{
						subAtt.State = en;
						subAtt.IsSelected = false;
						Work.Attendance.Update(SelectedAttendance);

					}
				}

				Work.Complete();
				IsLoading = false;

				ResetAttendance();
			});
		}
	

		private static AttendanceEnums GetType(string type)
		{
			return type switch
			{
				"Present" => AttendanceEnums.IsPresent,
				"NotPresent" => AttendanceEnums.NotPresent,
				"Sick" => AttendanceEnums.Excused,
				_ => AttendanceEnums.New,
			};
		}

		private async void AddNewAttendance()
		{
			IsLoading = true;
			MainGroup mainGroup = Work.MainGroup.Get(CurrentMainGroupId);

			var form = IsLectureSelected ? AttendanceFormEnums.Lecture : AttendanceFormEnums.Seminar;

			IEnumerable<Student> students;
			Attendance attendance = null;
			await System.Threading.Tasks.Task.Run(() =>
			{
				if (!IsLectureSelected)
				{
					students = Work.Students.GetStudentsBySubGroup(SelectedSubGroup);
					attendance = new Attendance()
					{
						Date = Time,
						Form = form,
						MainGroup = mainGroup,
						StudyForm = mainGroup.Form,
						SubGroup = SelectedSubGroup,
					};
				}
				else
				{
					students = Work.Students.GetStudentsByGroup(Work.MainGroup.Get(CurrentMainGroupId));
					attendance = new Attendance()
					{
						Date = Time,
						Form = form,
						MainGroup = mainGroup,
						StudyForm = mainGroup.Form
					};
				}

				foreach (var student in students)
				{
					if (attendance.SubAttendances == null)
					{
						attendance.SubAttendances = new List<SubAttendances>();
					}
					attendance.SubAttendances.Add(
						new SubAttendances()
						{
							Student = new Student(),
							State = AttendanceEnums.New
						});

					attendance.SubAttendances.Last().Student = student;
				}

				var cultureInfo = new CultureInfo("sk-SK");
				var dateTimeInfo = cultureInfo.DateTimeFormat;
				attendance.Day = dateTimeInfo.GetDayName(attendance.Date.DayOfWeek).ToUpper();

				if (IsLectureSelected)
				{
					Lectures = Attendances;
				}
				else
				{
					Seminars = Attendances;
				}

				Work.Attendance.Add(attendance);
				Work.Complete();
				IsLoading = false;

			});

			Attendances.Add(attendance);
		}

		private async void RemoveAttendance(string? id)
		{
			IsLoading = true;
			await System.Threading.Tasks.Task.Run(() =>
			{
				Work.Attendance.Get(id).SubAttendances.RemoveAt(0);
				Work.Attendance.Remove(Work.Attendance.Get(id));
				Work.Complete();

				if (IsLectureSelected)
				{
					Lectures = Attendances;
				}
				else
				{
					Seminars = Attendances;
				}
				IsLoading = false;
			});
							
			Attendances.Remove(Attendances.FirstOrDefault(x => x.Id == id));

		}
	}
}
