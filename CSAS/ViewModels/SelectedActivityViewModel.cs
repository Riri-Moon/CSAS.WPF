namespace CSAS.ViewModels
{
	public class SelectedActivityViewModel : BaseViewModelBindableBase
	{
		public Action CloseAction { get; set; }
		
		//Command definitions
		public DelegateCommand SaveChangesCommand { get; }
		public DelegateCommand MoveNextCommand { get; }
		public DelegateCommand MovePrevCommand { get; }
		
		private ObservableCollection<Task> _tasks;
		public ObservableCollection<Task> Tasks
		{
			get => new ObservableCollection<Task>(_tasks.OrderByDescending(x => x.Name));
			set => SetProperty(ref _tasks, value);
		}

		private Activity _activity;
		public Activity Activity
		{
			get { return _activity; }
			set => SetProperty(ref _activity, value);
		}

		public SelectedActivityViewModel(string currentGroupId, string activityId)
		{
			CurrentMainGroupId = currentGroupId;
			Activity = new Activity();
			Work = UoWSingleton.Instance;
			Activity = Work.Activity.Get(activityId);
		
			//Command initialization
			MoveNextCommand = new DelegateCommand(MoveNext);
			MovePrevCommand = new DelegateCommand(MovePrevious);
			SaveChangesCommand = new DelegateCommand(SaveChanges);
		}

		public Task? SelectedItem
		{
			get => _selectedItem;
			set => SetProperty(ref _selectedItem, value);
		}

		private void SaveChanges()
		{
			Activity.Modified = DateTime.Now;
			Work.Activity.Update(Activity);

			Work.Complete();
		}

		private void MovePrevious()
		{
			if (SelectedIndex > 0)
				SelectedIndex--;
		}
		private void MoveNext()
{
			if (SelectedIndex < Activity.Tasks.Count - 1)
				SelectedIndex++;
		}
		public int SelectedIndex
		{
			get => _selectedIndex;
			set => SetProperty(ref _selectedIndex, value);
		}
		private Task? _selectedItem;
		private int _selectedIndex;
	}
}
