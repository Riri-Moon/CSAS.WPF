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
			set
			{
				if (value != null)
				{
					ActivitiesForExport = new ObservableCollection<Activity>(ActivitiesForExport.Where(x => x.Name == value.Name && x.Deadline == value.Deadline));
				}
				SetProperty(ref _selectedActivity, value);
			}
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
			set
			{
				if (value != null && value.SubAttendances != null)
				{
					AttendancesForExport = new ObservableCollection<Attendance>(AttendancesForExport.Where(x => x.Date == value.Date && x.SubAttendances == value.SubAttendances));
				}
				SetProperty(ref _SelectAdattendance, value);
			}
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
			set
			{
				if (IsActivity)
				{
					SelectedActivity = new();
					IsSelectActivity = false;
				}
				SetProperty(ref _selectedStudent, value);
			}
		}

		private bool _isAll = true;
		public bool IsAll
		{
			get => _isAll;
			set
			{
				if (value && IsActivity)
				{
					var activities = GetActivities(Work.Activity.GetAll().Where(x => x.Student.MainGroup.Id == CurrentMainGroupId));
					ActivitiesForExport = new ObservableCollection<Activity>(Work.Activity.GetAll().Where(x => x.Student.MainGroup.Id == CurrentMainGroupId));
					Activities = new ObservableCollection<Activity>(activities.DistinctBy(x => new { x.Name, x.Deadline }));
					SetProperty(ref _isSelectActivity, value);
				}
				if (IsAttendance)
				{
					SetAttendance();					
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
				SetAttendance();
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
				SetAttendance();
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
				SetAttendance();
			}
		}

		private bool _isAssessment;
		
		public bool IsSelectAttendance
		{
			get => _isSelectAttendance;
			set
			{
				SetProperty(ref _isSelectAttendance, value);
				SetAttendance();

				Attendances = new ObservableCollection<Attendance>(AttendancesForExport.DistinctBy(x => new { x.Date }));
			}
		}
		private bool _isSelectAttendance = false;

		private bool _isSelectActivity = false;
		public bool IsSelectActivity
		{
			get => _isSelectActivity;
			set
			{
				List<Activity> newActivities = new();
				if (IsGroup && SelectedGroup != null)
				{
					 newActivities = GetActivities(Work.Activity.GetAll().Where(x => x.Student.SubGroup == SelectedGroup));
				
					SetProperty(ref _isSelectActivity, value);
				}
				else if (IsStudent && SelectedStudent != null)
				{
					 newActivities = Work.Activity.GetAll().Where(x => x.Student == SelectedStudent).ToList();

					SetProperty(ref _isSelectActivity, value);
				}
				else if (IsAll)
				{
					 newActivities = GetActivities(Work.Activity.GetAll().Where(x => x.Student.MainGroup.Id == CurrentMainGroupId));
				}
				ActivitiesForExport = new ObservableCollection<Activity>(Work.Activity.GetAll().Where(x => x.Student.MainGroup.Id ==  CurrentMainGroupId));
				Activities = new ObservableCollection<Activity>(newActivities.DistinctBy(x => new { x.Name, x.Deadline }));
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
		private static List<Activity> GetActivities(IEnumerable<Activity> activities)
		{
			List<Activity> newActivities = new();

			var temp = activities.GroupBy(x => new { x.Name, x.Deadline }).Where(y => y.Count() != 1);

			foreach (var grp in temp)
			{
				var act = activities.Where(x => x.Name == grp.Key.Name && x.Deadline == grp.Key.Deadline);
				if (act.Any())
				{
					newActivities.AddRange(act);
				}
			}
			return newActivities;
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
		private void SetAttendance() 
		{
			if (IsAll)
			{
				AttendancesForExport = new ObservableCollection<Attendance>(GetAttendances(Work.Students.GetStudentsByGroup(Work.MainGroup.Get(CurrentMainGroupId)).ToList()));
			}
			else if (IsGroup && SelectedGroup != null)
			{
				AttendancesForExport = new ObservableCollection<Attendance>(GetAttendances(Work.Students.GetStudentsBySubGroup(SelectedGroup).ToList()));
			}
			else if (IsStudent && SelectedStudent != null && SelectedStudent.Name != null)
			{
				var list = new List<Student>
				{
					SelectedStudent
				};
				AttendancesForExport = new ObservableCollection<Attendance>(GetAttendances(list));
			}

			if(AttendancesForExport!=null)
			Attendances = new ObservableCollection<Attendance>(AttendancesForExport
					.DistinctBy(x => new { x.Date }));
		}
		private List<Attendance> GetAttendances(List<Student> students)
		{
			var atte = new List<Attendance>();
			if (IsAllAttendances)
			{
				foreach (var stud in students)
				{
					if (stud.SubAttendances!= null && stud.SubAttendances.Any())
					{
						var subAtts = stud.SubAttendances.Select(x => x.Attendance).ToList();
						if (subAtts != null && subAtts.Any())
							atte.AddRange(subAtts);
					}
				}
			}
			else
			{
				var form = IsLecture ? Enums.Enums.AttendanceFormEnums.Lecture : Enums.Enums.AttendanceFormEnums.Seminar;
				foreach (var stud in students)
				{
					if (stud.SubAttendances != null && stud.SubAttendances.Any())
					{
						var subAtts = stud.SubAttendances.Select(x => x.Attendance).Where(x => x.Form == form).ToList();
						if (subAtts != null && subAtts.Any())
							atte.AddRange(subAtts);
					}
				}
			}

			return atte;
		}
	}
}
