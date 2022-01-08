namespace CSAS.ViewModels
{
	public class SettingsViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand SaveCommand { get; }

		private Settings _settings;

		public Settings Settings
		{

			get { return _settings; }
			set
			{
				SetProperty(ref _settings, value);
			}
		}

		public new UnitOfWork Work { get; set; }
		public SettingsViewModel(int currGroupId, ref AppDbContext context)
		{
			Work = new UnitOfWork(context);
			Settings = new Settings();

			if (Work.Settings.GetAll().FirstOrDefault() == null)
			{
				Work.Settings.Add(Settings);
				Work.Complete();
			}
			Settings = Work.Settings.GetAll().FirstOrDefault();
			SaveCommand = new DelegateCommand(SaveSettings);
		}

		private void SaveSettings()
		{
			Work.Settings.Update(Settings);
			Work.Complete();
		}
	}
}
