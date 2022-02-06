namespace CSAS.ViewModels
{
	public class SelectedActivityViewModel : BaseViewModelBindableBase
	{
		public Action CloseAction { get; set; }
		public DelegateCommand SaveChangesCommand { get; }
		public DelegateCommand MoveNextCommand { get; }
		public DelegateCommand MovePrevCommand { get; }
		public new UnitOfWork Work { get; set; }

		private ObservableCollection<Task> _tasks;
		public ObservableCollection<Task> Tasks
		{
			get { return _tasks; }
			set
			{
				SetProperty(ref _tasks, value);
			}
		}

		private Activity _activity;
		public Activity Activity
		{
			get { return _activity; }
			set
			{
				SetProperty(ref _activity, value);
			}
		}

		public SelectedActivityViewModel(string currentGroupId, string activityId)
		{
			CurrentMainGroupId = currentGroupId;
			Activity = new Activity();
			Work = UoWSingleton.Instance;
			Activity = Work.Activity.Get(activityId);
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
