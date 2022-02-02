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

		public new AppDbContext AppDbContext;
		public void SetCurrentGroup(string id)
		{
			HomeViewModel.CurrentMainGroupId = id;
		}

		public ApplicationViewModel(string id)
		{
			AppDbContext = new AppDbContext();
			CurrentMainGroupId = id;
			HomeViewModel = new HomeViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			AttendanceViewModel = new AttendanceViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			ActivityTemplateViewModel = new ActivityTemplateViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			ActivityViewModel = new ActivityViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			SettingsViewModel = new SettingsViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			FinalAssessmentViewModel = new FinalAssessmentViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			ExportViewModel = new ExportViewModel(id, ref AppDbContext)
			{
				CurrentMainGroupId = id
			};

			StatisticsViewModel = new StatisticsViewModel(id, ref AppDbContext);
			{
				CurrentMainGroupId = id;
			}
					
		}
	}
}
