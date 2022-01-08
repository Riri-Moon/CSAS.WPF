 namespace CSAS.ViewModels
{
	public class BaseDataViewModel : BaseViewModelBindableBase
	{
		private ObservableCollection<Activity> _activities;
		public ObservableCollection<Activity> Activities
		{
			get => _activities;
			set => SetProperty(ref _activities, value);
		}

		private ObservableCollection<Activity> _activitiesForExport;
		public ObservableCollection<Activity> ActivitiesForExport
		{
			get => _activitiesForExport;
			set => SetProperty(ref _activitiesForExport, value);
		}

		private Activity _selectedActivity;
		public Activity SelectedActivity
		{
			get => _selectedActivity;
			set => SetProperty(ref _selectedActivity, value);
		}

		private ObservableCollection<Attendance> _attendances;
		public ObservableCollection<Attendance> Attendances
		{
			get => _attendances;
			set => SetProperty(ref _attendances, value);
		}

		private ObservableCollection<Attendance> _attendancesForExport;
		public ObservableCollection<Attendance> AttendancesForExport
		{
			get => _attendancesForExport;
			set => SetProperty(ref _attendancesForExport, value);
		}

		private Attendance _SelectAdattendance;
		public Attendance SelectedAttendance
		{
			get => _SelectAdattendance;
			set => SetProperty(ref _SelectAdattendance, value);
		}

		private SubGroup _selectedGroup;
		public SubGroup SelectedGroup
		{
			get => _selectedGroup;
			set => SetProperty(ref _selectedGroup, value);
		}

		private ObservableCollection<SubGroup> _groups = new();
		public ObservableCollection<SubGroup> Groups
		{
			get => _groups;
			set => SetProperty(ref _groups, value);
		}
		protected new UnitOfWork Work { get; set; }

		private ObservableCollection<Student> _students;
		public ObservableCollection<Student> Students
		{
			get => _students;
			set => SetProperty(ref _students, value);
		}

		private Student _selectedStudent = new();
		public Student SelectedStudent
		{
			get => _selectedStudent;
			set => SetProperty(ref _selectedStudent, value);
		}


		private bool _isAll = true;
		public bool IsAll
		{
			get => _isAll;
			set
			{
				if (IsAll && IsActivity)
				{
					var activities = Work.Activity.GetAll().Where(x => x.Student.MainGroup == Work.MainGroup.Get(CurrentMainGroupId));
					ActivitiesForExport = new ObservableCollection<Activity>(activities);
					Activities = new ObservableCollection<Activity>(activities.DistinctBy(x => new { x.Name, x.Deadline }));
					SetProperty(ref _isSelectActivity, value);
				}
				if (IsAllAttendances && IsAll)
				{
					var att = Work.Attendance.GetAll()
					.Where(x => x.MainGroup == Work.MainGroup.Get(CurrentMainGroupId));

					AttendancesForExport = new ObservableCollection<Attendance>(att);
					Attendances = new ObservableCollection<Attendance>(att
					.DistinctBy(x => new { x.Date }));
				}
				Students = new ObservableCollection<Student>(Work.Students.GetStudentsByGroup(Work.MainGroup.Get(CurrentMainGroupId)));
				SetProperty(ref _isAll, value);
			}
		}
		private bool _isGroup;
		public bool IsGroup
		{
			get => _isGroup;
			set => SetProperty(ref _isGroup, value);
		}
		public bool IsStudent
		{
			get => _isStudent;
			set => SetProperty(ref _isStudent, value);
		}
		private bool _isStudent;
		public bool IsActivity
		{
			get => _isActivity;
			set => SetProperty(ref _isActivity, value);
		}
		private bool _isActivity = true;
		public bool IsAttendance
		{
			get => _isAttendance;
			set => SetProperty(ref _isAttendance, value);
		}
		private bool _isAttendance;
		public bool IsAssessment
		{
			get => _isAssessment;
			set => SetProperty(ref _isAssessment, value);
		}
	
		private bool _isAllAttendances = true;
		public bool IsAllAttendances
		{
			get => _isAllAttendances;
			set
			{
				IsSelectAttendance = false;
				SelectedAttendance = new();
				SetProperty(ref _isAllAttendances, value);
			}
		}

		private bool _isSeminar;
		public bool IsSeminar
		{
			get => _isSeminar;
			set
			{
				IsSelectAttendance = false;
				SelectedAttendance = null;
				SetProperty(ref _isSeminar, value);
			}
		}
		private bool _isLecture;
		public bool IsLecture
		{
			get => _isLecture;
			set
			{
				IsSelectAttendance = false;
				SelectedAttendance = null;
				SetProperty(ref _isLecture, value);
			}
		}

		private bool _isAssessment;

		public bool IsSelectAttendance
		{
			get => _isSelectAttendance;
			set
			{
				if (value)
				{
					if (IsAll)
					{
						if (IsLecture)
						{
							var att = Work.Attendance.GetAll()
								   .Where(x => x.MainGroup == Work.MainGroup.Get(CurrentMainGroupId) && x.Form == Enums.Enums.AttendanceFormEnums.Lecture);
							AttendancesForExport = new ObservableCollection<Attendance>(att);

							Attendances = new ObservableCollection<Attendance>(att.DistinctBy(x => new { x.Date }));
						}
						else
						{
							var att = Work.Attendance.GetAll()
								.Where(x => x.MainGroup == Work.MainGroup.Get(CurrentMainGroupId) && x.Form == Enums.Enums.AttendanceFormEnums.Seminar);
							AttendancesForExport = new ObservableCollection<Attendance>(att);
							Attendances = new ObservableCollection<Attendance>(att.DistinctBy(x => new { x.Date }));
						}
						SetProperty(ref _isSelectAttendance, value);
					}
					else if (IsGroup && SelectedGroup != null)
					{
						if (IsLecture)
						{
							List<Attendance>? att2 = new(Work.Attendance.GetAll()
								.Where(x => x.SubGroup == null));
							IEnumerable<SubAttendances>? att3 = att2.SelectMany(x => x.SubAttendances
							.Where(y => y.Student.SubGroup == SelectedGroup));
							AttendancesForExport = new ObservableCollection<Attendance>(att3.Select(x => x.Attendance));

							Attendances = new ObservableCollection<Attendance>(att3
								.Select(x => x.Attendance)
								.DistinctBy(y => y.Date));
						}
						else
						{
							IEnumerable<Attendance>? att = Work.Attendance.GetAll()
								 .Where(x => x.SubGroup == Work.SubGroup.Get(SelectedGroup.Id));
							AttendancesForExport = new ObservableCollection<Attendance>(att);
							Attendances = new ObservableCollection<Attendance>(att
								.DistinctBy(x => new { x.Date }));
						}

						SetProperty(ref _isSelectAttendance, value);
					}
					else if (SelectedStudent != null)
					{
						if (IsLecture)
						{
							IEnumerable<Attendance>? att = SelectedStudent.SubAttendances.Where(x => x.Attendance.Form == Enums.Enums.AttendanceFormEnums.Lecture).Select(x => x.Attendance);
							AttendancesForExport = new ObservableCollection<Attendance>(att);
							Attendances = new ObservableCollection<Attendance>(att);
						}
						else if (IsSeminar)
						{
							IEnumerable<Attendance>? att = SelectedStudent.SubAttendances.Where(x => x.Attendance.Form == Enums.Enums.AttendanceFormEnums.Seminar).Select(x => x.Attendance);
							AttendancesForExport = new ObservableCollection<Attendance>(att);
							Attendances = new ObservableCollection<Attendance>(att);
						}
						SetProperty(ref _isSelectAttendance, value);
					}
					else
					{
						SetProperty(ref _isSelectAttendance, false);
					}
				}
				else
				{
					if (IsAllAttendances && IsGroup && SelectedGroup != null)
					{
						var atte = new List<Attendance>(Work.Attendance.GetAll()
						.Where(x => x.SubGroup == Work.SubGroup.Get(SelectedGroup.Id)));
						var atte1 = Work.Attendance.GetAll().Where(x => x.SubGroup == null && x.MainGroup.Id == CurrentMainGroupId).SelectMany(x => x.SubAttendances.Where(y => y.Student.SubGroup == SelectedGroup)).Select(x => x.Attendance);

						foreach (var att in atte1)
						{
							atte.Add(att);
						}
						AttendancesForExport = new ObservableCollection<Attendance>(atte);

						var att1 = new List<Attendance>(atte.DistinctBy(x => new { x.Date }));

						var att2 = atte1.DistinctBy(s => s.Date);

						foreach (var att in att2)
						{
							att1.Add(att);
						}
						Attendances = Attendances = new ObservableCollection<Attendance>(att1);
					}
					//All Lectures and Seminars for All students

					if (IsAllAttendances && IsStudent && SelectedStudent != null)
					{
						var att = SelectedStudent.SubAttendances.Select(x => x.Attendance);

						AttendancesForExport = new ObservableCollection<Attendance>(att);
						Attendances = new ObservableCollection<Attendance>(att);
					}
					value = false;
					SetProperty(ref _isSelectAttendance, value);
				}
			}
		}
		private bool _isSelectAttendance = false;

		private bool _isSelectActivity = false;
		public bool IsSelectActivity
		{
			get => _isSelectActivity;
			set
			{
				if (IsGroup && SelectedGroup != null)
				{
					var activities = Work.Activity.GetAll().Where(x => x.Student.SubGroup == Work.SubGroup.Get(SelectedGroup.Id));
					ActivitiesForExport = new ObservableCollection<Activity>(activities);
					Activities = new ObservableCollection<Activity>(activities.DistinctBy(x => new { x.Name, x.Deadline }));
					SetProperty(ref _isSelectActivity, value);
				}
				else if (IsStudent && SelectedStudent != null)
				{
					var activities = Work.Activity.GetAll().Where(x => x.Student == Work.Students.Get(SelectedStudent.Id));
					ActivitiesForExport = new ObservableCollection<Activity>(activities);
					Activities = new ObservableCollection<Activity>(activities.DistinctBy(x => new { x.Name, x.Deadline }));
					SetProperty(ref _isSelectActivity, value);
				}

				SetProperty(ref _isSelectActivity, value);
			}
		}

		protected IList<FinalAssessment> GetAssessments()
		{
			var list = new List<FinalAssessment>();
			var students = GetStudents();

			foreach (Student student in students)
			{
				list.Add(Work.FinalAssessment.GetAll().FirstOrDefault(x => x.Student == student));
			}

			return list;
		}

		protected List<Student> GetStudents()
		{
			if (IsAll)
			{
				return Students.ToList();
			}
			else if (IsGroup && SelectedGroup != null)
			{
				return Students.Where(x => x.SubGroup == SelectedGroup).ToList();
			}
			else if (IsStudent && SelectedStudent != null)
			{
				var list = new List<Student>
				{
					Students.FirstOrDefault(x => x.Id == SelectedStudent.Id)
				};
				return list;
			}

			return null;
		}
	}
}
