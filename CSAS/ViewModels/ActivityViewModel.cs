using CSAS.Helpers;
using Microsoft.Win32;
using System.Net.Mail;
using Process = System.Diagnostics;

namespace CSAS.ViewModels
{
	public class ActivityViewModel : BaseViewModelBindableBase
	{
		public static Logger _logger = new();
		public DelegateCommand RefreshCommand { get; }
		public DelegateCommand<string?> SelectTemplateCommand { get; }
		public DelegateCommand CreateActivityCommand { get; }
		public DelegateCommand SelectAttachmentsCommand { get; }
		public DelegateCommand<string> RemoveAttachmentCommand { get; }
		public DelegateCommand<string> OpenAttachmentCommand { get; }

		private bool _isSelectAll;
		public bool IsSelectAll
		{
			get => _isSelectAll;
			set => SetProperty(ref _isSelectAll, value);
		}
		private bool _isSelectGroup;
		public bool IsSelectGroup
		{
			get => _isSelectGroup;
			set => SetProperty(ref _isSelectGroup, value);
		}

		private bool _isSelectIndividual;
		public bool IsSelectIndividual
		{
			get => _isSelectIndividual;
			set => SetProperty(ref _isSelectIndividual, value);
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
		private ObservableCollection<Attachments> _attachments = new();
		public ObservableCollection<Attachments> Attachments
		{
			get => _attachments;
			set => SetProperty(ref _attachments, value);
		}

		private ObservableCollection<Student> _students;
		public ObservableCollection<Student> Students
		{
			get => _students;
			set => SetProperty(ref _students, value);
		}

		private ObservableCollection<ActivityTemplate> _templates;
		public ObservableCollection<ActivityTemplate> Templates
		{
			get => _templates;
			set => SetProperty(ref _templates, value);
		}

		private Activity _activity;
		public Activity Activity
		{
			get => _activity;
			set => SetProperty(ref _activity, value);
		}

		private ActivityTemplate _activityTemplate;
		public ActivityTemplate ActivityTemplate
		{
			get => _activityTemplate;
			set => SetProperty(ref _activityTemplate, value);
		}

		private DateTime _date;
		public DateTime Date
		{
			get => _date;
			set => SetProperty(ref _date, value);
		}

		private Student _student = new();
		public Student Student
		{
			get => _student;
			set => SetProperty(ref _student, value);
		}

		public ActivityViewModel(string currentGroupId)
		{
			Work = UoWSingleton.Instance;

			Students = new ObservableCollection<Student>(Work.Students.GetAll().Where(x => x.MainGroup.Id == currentGroupId));
			Groups = new ObservableCollection<SubGroup>(Work.SubGroup.GetAll().Where(g => g.MainGroup.Id == currentGroupId));
			RefreshTemplates();
			Activity = new Activity
			{
				Deadline = DateTime.Now,
				Attachments = new List<Attachments>()
			};

			RefreshCommand = new(RefreshTemplates);
			SelectTemplateCommand = new DelegateCommand<string?>(SelectActivityTemplate);
			CreateActivityCommand = new(CreateActivity);
			SelectAttachmentsCommand = new(SelectAttachments);
			RemoveAttachmentCommand = new DelegateCommand<string>(RemoveAttachment);
			OpenAttachmentCommand = new DelegateCommand<string>(OpenAttachment);
		}

		private void RefreshTemplates()
		{
			Templates = new ObservableCollection<ActivityTemplate>(Work.ActivityTemplate.GetAll().ToList());
		}

		private void SelectActivityTemplate(string? id)
		{
			ActivityTemplate = Templates.FirstOrDefault(x => x.Id == id);
			Activity.Name = ActivityTemplate.Name;
			Activity.Tasks = GetTasksFromTemplate(ActivityTemplate, Activity);
		}
		// Add send email
		private void CreateActivity()
		{
			try
			{
				if (Activity == null || string.IsNullOrEmpty(Activity.Name) || !GetStudents().Any() || GetStudents().FirstOrDefault() == null)
				{
					MessageBoxHelper.Show("", "Nie je vybraná žiadna aktivita alebo študent", true);
					return;
				}

				List<Activity> activity = new();
				MailAddressCollection mailAddresses = new();
				MailAddressCollection ccMailAdresses = new();
				if (Activity.Attachments == null)
				{
					Activity.Attachments = new();
				}
				Activity.Attachments.AddRange(Attachments);
				Activity.Created = DateTime.Now;
				Activity.Modified = Activity.Created;
				string? subject = $"[{Work.MainGroup.Get(CurrentMainGroupId).Subject}] Nová úloha - {Activity.Name}";
				string? body = $"Dobrý deň, <br/><br/> práve Vám bola pridelená nová úloha s názvom {Activity.Name}. <br/> Dátum odovzdania je { Activity.Deadline.ToShortDateString() } {Activity.Deadline.ToShortTimeString()}." +
					$"<br/><br/> V prípade akýchkoľvek otázok ma neváhajte kontaktovať";

				foreach (var student in GetStudents().Where(x => x != null))
				{
					Activity act = new();
					act = Activity.Clone();
					act.Student = student;
					act.Tasks = new List<Task>();

					foreach (var task in Activity.Tasks)
					{
						act.Tasks.Add(task.Clone());
					}

					activity.Add(act);
					mailAddresses.Add(student.SchoolEmail);
					ccMailAdresses.Add(student.Email);
				}
				Work.Activity.AddRange(activity);
				Work.Complete();

				List<Attachment> attachments = new();

				if (Activity.IsSendEmail)
				{
					OutlookService outlookService = new();
					var signature = string.Empty;
					var settings = Work.Settings.GetSettingsByMainGroup(activity.FirstOrDefault().Student.MainGroup.Id);
					if (settings != null && settings.Signature != null)
					{
						signature = settings.Signature;
					}
					var isFinished = outlookService.SendEmail(subject, mailAddresses, ccMailAdresses, body, Activity.Attachments.Select(x => x.PathToFile).ToList(), false, signature);
				}

				Activity = new();
				Activity.Deadline = DateTime.Now;
				ActivityTemplate = new();
			}
			catch(Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
		}

		private List<Student> GetStudents()
		{
			List<Student> students = new();
			if (IsSelectAll)
			{
				return Students.ToList();
			}
			else if (IsSelectGroup)
			{
				return Students.Where(x => x.SubGroup == SelectedGroup).ToList();
			}
			else
			{
				students.Add(Students.FirstOrDefault(x => x == Student));
				return students;
			}
		}

		private static List<Task> GetTasksFromTemplate(ActivityTemplate activityTemplate, Activity act)
		{
			List<Task> tasks = new();

			foreach (var tskTemp in activityTemplate.TasksTemplate)
			{
				var task = new Task
				{
					Activity = act,
					Name = tskTemp.Name,
					MaxPoints = tskTemp.MaxPoints,
					CreateDate = DateTime.Now
				};
				tasks.Add(task);
			}
			return tasks;
		}

		private void SelectAttachments()
		{
			OpenFileDialog openFileDialog = new();
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == true)
			{
				foreach (var file in openFileDialog.FileNames)
				{
					Attachments att = new();
					att.Activity = Activity;
					att.PathToFile = file;
					Attachments.Add(att);
				}
			}
		}
		private void RemoveAttachment(string path)
		{
			Attachments.Remove(Attachments.Where(x => x.PathToFile == path).FirstOrDefault());
		}
		private void OpenAttachment(string path)
		{
			var p = new Process.Process
			{
				StartInfo = new Process.ProcessStartInfo(path)
				{
					UseShellExecute = true
				}
			};
			p.Start();
		}
	}
}


