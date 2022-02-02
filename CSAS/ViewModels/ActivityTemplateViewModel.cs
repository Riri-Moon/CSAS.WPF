namespace CSAS.ViewModels
{
	//[ArtifactsPath(@"C:\Users\ZZ03XZ693\source\repos\MauiApp1")]
	//[MemoryDiagnoser]
	//[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Method)]
	//[RankColumn]  
	//[SimpleJob(RunStrategy.Monitoring)]
	//[SkewnessColumn, KurtosisColumn]
	public class ActivityTemplateViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand AddTaskCommand { get; }
		public DelegateCommand RemoveTasksCommand { get; }
		public DelegateCommand SaveTemplateCommand { get; }
		public DelegateCommand<string?> DeleteTemplateCommand { get; }
		public DelegateCommand<string?> EditTemplateCommand { get; }
		public DelegateCommand<string?> CopyTemplateCommand { get; }

		private new UnitOfWork Work { get; set; }

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

		private ObservableCollection<TaskTemplate> _tasks;
		public ObservableCollection<TaskTemplate> Tasks
		{
			get => _tasks;
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

		public ActivityTemplateViewModel(string currentGroupId, ref AppDbContext context)
		{
			Work = new UnitOfWork(context);
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
		// Use for Benchmarking of the functions
		//public void RunTask()
		//{
		//    var result = BenchmarkRunner.Run<ActivityTemplateViewModel>();
		//}

		//[Benchmark]
		public void AddNewTaskTemplate()
		{
			Tasks.Add(
				new TaskTemplate()
				{
					ActivityTemplate = NewTemplate,
				});

			IsSelectAllVisible = true;
		}
		//[Benchmark]
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

			var tasks = Work.TasksTemplate.GetAll().Where(x => x.ActivityTemplate.Id == id);
			foreach (var task in tasks)
			{
				var taskClone = task.Clone();
				taskClone.Id = new Guid().ToString();
				Tasks.Add(taskClone);
			}

			NewTemplate.Id = new Guid().ToString();
		}

		private void EditTemplate(string? id)
		{
			NewTemplate = Work.ActivityTemplate.Get(id);
			NewTemplate.IsUpdate = true;
			Tasks = new ObservableCollection<TaskTemplate>(NewTemplate.TasksTemplate);
		}

		private void RemoveTemplate(string? id)
		{
			var act = Work.ActivityTemplate.Get(id);
			Work.TasksTemplate.RemoveRange(act.TasksTemplate);
			Work.ActivityTemplate.Remove(act);
			Work.Complete();

			RefreshTemplateList();
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
