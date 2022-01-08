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
		public DelegateCommand<int?> DeleteTemplateCommand { get; }
		public DelegateCommand<int?> EditTemplateCommand { get; }
		public DelegateCommand<int?> CopyTemplateCommand { get; }

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

		public ActivityTemplateViewModel(int currentGroupId, ref AppDbContext context)
		{
			Work = new UnitOfWork(context);
			Templates = new ObservableCollection<ActivityTemplate>(Work.ActivityTemplate.GetAll().ToList());

			AddTaskCommand = new DelegateCommand(AddNewTaskTemplate);
			RemoveTasksCommand = new DelegateCommand(RemoveSelectedTasks);
			SaveTemplateCommand = new DelegateCommand(SaveTaskTemplate);

			EditTemplateCommand = new DelegateCommand<int?>(EditTemplate);
			DeleteTemplateCommand = new DelegateCommand<int?>(RemoveTemplate);
			CopyTemplateCommand = new DelegateCommand<int?>(CopyTemplate);

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

		private void CopyTemplate(int? id)
		{
			NewTemplate = Work.ActivityTemplate.Get(id.Value).Clone();
			Tasks = new ObservableCollection<TaskTemplate>();

			var tasks = Work.TasksTemplate.GetAll().Where(x => x.ActivityTemplate.Id == id.Value);
			foreach (var task in tasks)
			{
				var taskClone = task.Clone();
				taskClone.Id = new int();
				Tasks.Add(taskClone);
			}

			NewTemplate.Id = new int();

		}

		private void EditTemplate(int? id)
		{
			NewTemplate = Work.ActivityTemplate.Get(id.Value);
			NewTemplate.IsUpdate = true;
			Tasks = new ObservableCollection<TaskTemplate>(NewTemplate.TasksTemplate);
		}

		private void RemoveTemplate(int? id)
		{
			var act = Work.ActivityTemplate.Get(id.Value);

			//_work.TaskTemplate.RemoveRange(_work.TaskTemplate.GetAll().Where(x => x.ActivityTemplate.Id==id.Value));
			Work.ActivityTemplate.Remove(act);
			Work.Complete();

			RefreshTemplateList();
		}

		private int GetMaxPoints()
		{
			int count = 0;
			// var templates = _work.TasksTemplate.GetAll().Where(x => x.ActivityTemplate.Id == template.Id);
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
