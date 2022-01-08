using CSAS.Helpers;

namespace CSAS.ViewModels
{
	public class SelectedActivityViewModel : BaseViewModelBindableBase
	{
		public Action CloseAction { get; set; }
		public DelegateCommand SaveChangesCommand { get; }
		public new UnitOfWork Work { get; set; }

		private ObservableCollection<Models.Task> _tasks;
		public ObservableCollection<Models.Task> Tasks
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

		public SelectedActivityViewModel(int currentGroupId, int activityId)
		{
			CurrentMainGroupId = currentGroupId;
			Activity = new Activity();
			AppDbContext appDbContext = new();
			Work = new UnitOfWork(appDbContext);
			Activity = Work.Activity.Get(activityId);

			SaveChangesCommand = new DelegateCommand(SaveChanges);
		}

		public Models.Task? SelectedItem
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

		public int SelectedIndex
		{
			get => _selectedIndex;
			set => SetProperty(ref _selectedIndex, value);
		}
		private Models.Task? _selectedItem;
		private int _selectedIndex;
	}
}
