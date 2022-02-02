using CSAS.Helpers;
using CSAS.Models;
using CSAS.Validators;
using CSAS.Views;
using System.Net.Mail;

namespace CSAS.ViewModels
{
	public class HomeViewModel : BaseViewModelBindableBase
	{
		readonly Logger _logger = new();
		private bool _isAddStudent;
		public bool IsAddStudent
		{
			get { return _isAddStudent; }
			set
			{
				SetProperty(ref _isAddStudent, value);
			}
		}
		private bool _isModifyStudent;
		public bool IsModifyStudent
		{
			get { return _isModifyStudent; }
			set
			{
				SetProperty(ref _isModifyStudent, value);
			}
		}
		public DelegateCommand SetSubGroupCommand { get; }
		public DelegateCommand<string?> ModifyStudentCommand { get; }
		public DelegateCommand<string?> OpenModifyStudentCommand { get; }
		public DelegateCommand AddNewStudentCommand { get; }
		public DelegateCommand<string?> RemoveStudentCommand { get; }
		public DelegateCommand<string?> ShowActivitiesCommand { get; }
		public DelegateCommand<string?> IndividualStudyCommand { get; }
		private ObservableCollection<Student> _students;
		public DelegateCommand ShowAddStudentCommand { get; }
		public DelegateCommand LoadStudentsCommand { get; }
		public DelegateCommand<string?> OpenActivityCommand { get; }
		public DelegateCommand<string?> SendEmailToStudentCommand { get; }
		public DelegateCommand<object?> GiveOnePointCommand { get; }
		public DelegateCommand SendEmailToAllCommand { get; }
		public ObservableCollection<Student> Students
		{
			get => _students;
			set => SetProperty(ref _students, value);
		}
		private SubGroup _selectedGroup;

		public SubGroup SelectedGroup
		{
			get => _selectedGroup;
			set => SetProperty(ref _selectedGroup, value);
		}

		private bool _isActivityClosed;

		public bool IsActivityClosed
		{
			get => _isActivityClosed;
			set
			{
				if (value)
				{
					RefreshStudents();
				}
				SetProperty(ref _isActivityClosed, value);
			}
		}
		private Student _newStudent = new();
		public Student NewStudent
		{
			get => _newStudent;
			set => SetProperty(ref _newStudent, value);
		}
		private Student _selectedStudent = new();
		public Student SelectedStudent
		{
			get => _selectedStudent;
			set => SetProperty(ref _selectedStudent, value);
		}
		private ObservableCollection<SubGroup> _subGroups;
		public ObservableCollection<SubGroup> SubGroups
		{
			get => _subGroups;
			set => SetProperty(ref _subGroups, value);
		}

		private ObservableCollection<Activity> _activities;
		public ObservableCollection<Activity> Activities
		{
			get => _activities;
			set => SetProperty(ref _activities, value);
		}
		private string CurrentStudentId { get; set; }
		public HomeViewModel(string currentGroupId, ref AppDbContext context)
		{
			Work = new UnitOfWork(context);
			CurrentMainGroupId = currentGroupId;

			LoadStudents();
			AppDbContext = context;
			IndividualStudyCommand = new DelegateCommand<string?>(ChangeIndividualStudy);
			ShowAddStudentCommand = new DelegateCommand(ShowAddStudent);
			AddNewStudentCommand = new DelegateCommand(AddNewStudent);
			RemoveStudentCommand = new DelegateCommand<string?>(RemoveStudent);
			OpenModifyStudentCommand = new DelegateCommand<string?>(LoadStudentToUpdate);
			ModifyStudentCommand = new DelegateCommand<string?>(UpdateStudent);
			SetSubGroupCommand = new DelegateCommand(GetSubGroup);
			LoadStudentsCommand = new DelegateCommand(RefreshStudents);
			ShowActivitiesCommand = new DelegateCommand<string?>(ShowActivities);
			OpenActivityCommand = new DelegateCommand<string?>(OpenActivity);
			SubGroups = new ObservableCollection<SubGroup>(Work.SubGroup.GetAll().Where(x => x.MainGroup == Work.MainGroup.Get(currentGroupId)).ToList());
			SendEmailToStudentCommand = new DelegateCommand<string?>(SendEmailToTheStudent);
			GiveOnePointCommand = new DelegateCommand<object?>(GivePoint);
			SendEmailToAllCommand = new DelegateCommand(SendEmailToAll);

		}

		public void GetSubGroup()
		{
			NewStudent.SubGroup = SelectedGroup;
		}

		public void LoadStudentToUpdate(string? id)
		{

			NewStudent = Students.FirstOrDefault(x => x.Id == id);
			SelectedGroup = NewStudent.SubGroup;
			IsAddStudent = true;
			IsModifyStudent = true;
		}
		public void UpdateStudent(string? id)
		{
			if (NewStudent != null && NewStudent.Id == id)
			{
				Work.Students.Update(NewStudent);
				Work.Complete();
			}
			IsAddStudent = false;
			IsModifyStudent = false;
			NewStudent = new Student();
		}
		public void RemoveStudent(string? id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				Work.Students.Remove(Work.Students.Get(id));
				Work.Complete();

				RefreshStudents();
			}
		}

		public void AddNewStudent()
		{
			try
			{
				if (StudentValidator.ValidateStudent(NewStudent))
				{
					NewStudent.MainGroup = Work.MainGroup.Get(CurrentMainGroupId);
					NewStudent.SubGroup = Work.SubGroup.Get(SelectedGroup.Id);
					Work.Students.Add(NewStudent);
					Work.Complete();
					Students.Add(NewStudent);
					NewStudent = new Student();
					IsAddStudent = false;
				}
				else
				{
					MessageBoxHelper.Show("Nesprávne vyplnené údaje", "Niektorý z údajov nie je správne vyplnený", true);
					return;
				}
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
			}
		}
		public void ShowAddStudent()
		{
			if (IsAddStudent)
			{
				IsAddStudent = false;
			}
			else
			{
				if (IsModifyStudent)
				{
					SelectedGroup = new SubGroup();
					NewStudent = new Student();
					IsModifyStudent = false;
				}
				IsAddStudent = true;
			}
		}
		public void LoadStudents()
		{
			var grp = Work.MainGroup.Get(CurrentMainGroupId);
			if (Students == null || !Students.Any())
			{
				Students = new ObservableCollection<Student>(Work.Students.GetStudentsByGroup(grp).ToList());
				var finalAssessments = Work.FinalAssessment.GetAll().Where(x => x.Student.MainGroup == grp);
				foreach (var student in Students)
				{
					if (student.FinalAssessment == null)
					{
						student.FinalAssessment = finalAssessments.FirstOrDefault(x => x.Student == student);
					}
				}
			}
		}

		public void RefreshStudents()
		{
			var grp = Work.MainGroup.Get(CurrentMainGroupId);

			Students = new ObservableCollection<Student>(Work.Students.GetStudentsByGroup(grp).ToList());
			var finalAssessments = Work.FinalAssessment.GetAll().Where(x => x.Student.MainGroup == grp);
			foreach (var student in Students)
			{
				if (student.FinalAssessment == null)
				{
					student.FinalAssessment = finalAssessments.FirstOrDefault(x => x.Student == student);
				}
			}

			IsActivityClosed = false;
		}
		public void ChangeIndividualStudy(string? studentId)
		{
			if (!string.IsNullOrEmpty(studentId))
			{
				Work.Students.Update(Students.FirstOrDefault(x => x.Id == studentId));
				Work.Complete();
			}
		}
		private void ShowActivities(string? id)
		{
			CurrentStudentId = id;
			SelectedStudent = new Student();
			SelectedStudent = Work.Students.Get(CurrentStudentId);
		}

		private void SendEmailToTheStudent(string? id)
		{
			OutlookService outlookService = new();

			MailAddressCollection collection = new()
			{
				new MailAddress(Work.Students.Get(id).SchoolEmail)
			};
			outlookService.SendEmail("", collection, null, "", null, true);
		}
		private void SendEmailToAll()
		{
			OutlookService outlookService = new();
			MailAddressCollection collection = new();

			foreach (var stud in Students)
			{
				collection.Add(new MailAddress(stud.SchoolEmail));
			}

			outlookService.SendEmail("", collection, null, "", null, true);
		}

		private void OpenActivity(string? id)
		{
			SelectedActivityWindow saw = new()
			{
				DataContext = new SelectedActivityViewModel(CurrentMainGroupId, id)
			};
			var result = saw.ShowDialog();

			AppDbContext = new AppDbContext();
			Work = new UnitOfWork(AppDbContext);
			string selStudId = SelectedStudent.Id;
			RefreshStudents();
			SelectedStudent = Students.FirstOrDefault(x => x.Id == selStudId);
		}
		private async void GivePoint(object? obj)
		{
			var param = obj as object[];
			string id = (string)param[0];
			string count = (string)param[1];
			IsLoading = true;
			await System.Threading.Tasks.Task.Run(() =>
			{
				var act = new Activity()
				{
					Name = "Body za aktivitu",
					Student = Work.Students.Get(id),
					Deadline = DateTime.Now,
					Tasks = new List<Task>()
					{
						 new Task()
						{
							 Name="Aktivita na hodine",
							 MaxPoints=int.Parse(count),
							 Points=int.Parse(count)
						}
					}
				};

				Work.Activity.Add(act);
				Work.Complete();
				RefreshStudents();
				IsLoading = false;
			});
		}
	}
}
