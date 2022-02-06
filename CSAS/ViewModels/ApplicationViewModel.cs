 namespace CSAS.ViewModels
{
	public class ApplicationViewModel : BaseViewModelBindableBase
	{
		public HomeViewModel HomeViewModel { get; set; }
		public AttendanceViewModel AttendanceViewModel { get; set; }
		public ActivityTemplateViewModel ActivityTemplateViewModel { get; set; }
		public ActivityViewModel ActivityViewModel { get; set; }
		public FinalAssessmentViewModel FinalAssessmentViewModel { get; set; }
		public SettingsViewModel SettingsViewModel { get; set; }
		public ExportViewModel ExportViewModel { get; set; }
		public StatisticsViewModel StatisticsViewModel { get; set; }
		public void SetCurrentGroup(string id)
		{
			HomeViewModel.CurrentMainGroupId = id;
		}

		public ApplicationViewModel(string id)
		{
			CurrentMainGroupId = id;
			HomeViewModel = new HomeViewModel(id)
			{
				CurrentMainGroupId = id
			};

			AttendanceViewModel = new AttendanceViewModel(id)
			{
				CurrentMainGroupId = id
			};

			ActivityTemplateViewModel = new ActivityTemplateViewModel(id)
			{
				CurrentMainGroupId = id
			};

			ActivityViewModel = new ActivityViewModel(id)
			{
				CurrentMainGroupId = id
			};

			SettingsViewModel = new SettingsViewModel(id)
			{
				CurrentMainGroupId = id
			};

			FinalAssessmentViewModel = new FinalAssessmentViewModel(id)
			{
				CurrentMainGroupId = id
			};

			ExportViewModel = new ExportViewModel(id)
			{
				CurrentMainGroupId = id
			};

			StatisticsViewModel = new StatisticsViewModel(id);
			{
				CurrentMainGroupId = id;
			}
					
		}
	}
}
