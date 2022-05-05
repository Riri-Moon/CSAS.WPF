namespace CSAS.ViewModels
{
	public class ActivityTemplateViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand AddTaskCommand { get; }
		public DelegateCommand RemoveTasksCommand { get; }
		public DelegateCommand SaveTemplateCommand { get; }
		public DelegateCommand<string?> DeleteTemplateCommand { get; }
		public DelegateCommand<string?> EditTemplateCommand { get; }
		public DelegateCommand<string?> CopyTemplateCommand { get; }

		private ObservableCollection<ActivityTemplate> _templates;
		public ObservableCollection<ActivityTemplate> Templates
		{
			get => _templates;
			set => SetProperty(ref _templates, value);
		}

		private ActivityTemplate _newTemplate;
		public ActivityTemplate NewTemplate
		{
			get => _newTemplate;
			set => SetProperty(ref _newTemplate, value);
		}

		private ObservableCollection<TaskTemplate> _tasks;//= new ObservableCollection<TaskTemplate>(_tasks.OrderByDescending(x=>x.CreateDate).ToList());
		public ObservableCollection<TaskTemplate> Tasks
		{
			get => _tasks;
			//get => new ObservableCollection<TaskTemplate>(_tasks.OrderByDescending(x=>x.CreateDate).ToList());
			set => SetProperty(ref _tasks, value);
		}

		private bool _isSelectAllVisible = false;
		public bool IsSelectAllVisible
		{
			get => _isSelectAllVisible;
			set => SetProperty(ref _isSelectAllVisible, value);
		}

		private bool _isSelectAll = false;
		public bool IsSelectAll
		{
			get => _isSelectAll;
			set
			{
				foreach (var task in Tasks)
				{
					task.IsSelected = value;
				}

				SetProperty(ref _isSelectAll, value);
			}
		}

		public ActivityTemplateViewModel(string currentGroupId)
		{
			Work = UoWSingleton.Instance;
			Templates = new ObservableCollection<ActivityTemplate>(Work.ActivityTemplate.GetAll().ToList());

			AddTaskCommand = new DelegateCommand(AddNewTaskTemplate);
			RemoveTasksCommand = new DelegateCommand(RemoveSelectedTasks);
			SaveTemplateCommand = new DelegateCommand(SaveTaskTemplate);

			EditTemplateCommand = new DelegateCommand<string?>(EditTemplate);
			DeleteTemplateCommand = new DelegateCommand<string?>(RemoveTemplate);
			CopyTemplateCommand = new DelegateCommand<string?>(CopyTemplate);

			NewTemplate = new ActivityTemplate();
			Tasks = new ObservableCollection<TaskTemplate>();
		}

		public void AddNewTaskTemplate()
		{
			Tasks.Add(
				new TaskTemplate()
				{
					ActivityTemplate = NewTemplate,
					CreateDate = DateTime.Now
				});

			IsSelectAllVisible = true;
		}
		public void RemoveSelectedTasks()
		{
			foreach (var task in Tasks.Where(x => x.IsSelected).ToList())
			{
				Tasks.Remove(task);
			}

			if (Tasks.Count == 0)
			{
				IsSelectAllVisible = false;
				IsSelectAll = false;
			}
		}
		//[Benchmark]
		public void SaveTaskTemplate()
		{
			if (NewTemplate.Validate())
			{
				if (!NewTemplate.IsUpdate)
				{
					NewTemplate.TasksTemplate = new List<TaskTemplate>();

					NewTemplate.TasksTemplate = Tasks;
					NewTemplate.Created = DateTime.Now;
					NewTemplate.MaxPoints = GetMaxPoints();

					Work.ActivityTemplate.Add(NewTemplate);
					Work.Complete();
				}
				else
				{
					NewTemplate.MaxPoints = GetMaxPoints();
					Work.ActivityTemplate.Update(NewTemplate);
					Work.Complete();
				}

				RefreshAll();
			}
		}

		private void CopyTemplate(string? id)
		{
			NewTemplate = Work.ActivityTemplate.Get(id).Clone();
			Tasks = new ObservableCollection<TaskTemplate>();
			NewTemplate.IsUpdate = false;
			var tasks = Work.TasksTemplate.GetAll().Where(x => x.ActivityTemplate.Id == id);
			foreach (var task in tasks)
			{
				var taskClone = task.Clone();
				taskClone.Id = Guid.NewGuid().ToString();
				Tasks.Add(taskClone);
			}

			Tasks = new ObservableCollection<TaskTemplate>(Tasks.OrderBy(x => x.CreateDate));
			NewTemplate.Id = Guid.NewGuid().ToString();
		}

		private void EditTemplate(string? id)
		{
			NewTemplate = Work.ActivityTemplate.Get(id);
			NewTemplate.IsUpdate = true;
			Tasks = new ObservableCollection<TaskTemplate>(NewTemplate.TasksTemplate.OrderBy(x=>x.CreateDate));
		}

		private void RemoveTemplate(string? id)
		{
			var act = Work.ActivityTemplate.Get(id);
			Work.TasksTemplate.RemoveRange(act.TasksTemplate);
			Work.ActivityTemplate.Remove(act);
			Work.Complete();

			RefreshTemplateList();
			NewTemplate = new ActivityTemplate();
		}

		private int GetMaxPoints()
		{
			int count = 0;
			foreach (var task in Tasks)
			{
				count += task.MaxPoints.Value;
			}

			return count;
		}

		private void RefreshAll()
		{
			NewTemplate = new ActivityTemplate();
			Tasks = new ObservableCollection<TaskTemplate>();
			Templates = new ObservableCollection<ActivityTemplate>(Work.ActivityTemplate.GetAll().ToList());
		}

		private void RefreshTemplateList()
		{
			Templates = new ObservableCollection<ActivityTemplate>(Work.ActivityTemplate.GetAll().ToList());
		}
	}
}
