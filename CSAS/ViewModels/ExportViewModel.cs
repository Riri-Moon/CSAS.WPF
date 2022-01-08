namespace CSAS.ViewModels
{
	public class ExportViewModel : BaseDataViewModel
	{
		public DelegateCommand ExportCommand { get; }

		private bool _isAnonymized = new();
		public bool IsAnonymized
		{
			get => _isAnonymized;
			set => SetProperty(ref _isAnonymized, value);
		}

		private bool _isExcel = true;
		public bool IsExcel
		{
			get => _isExcel;
			set => SetProperty(ref _isExcel, value);
		}
		private bool _isSendMe;
		public bool IsSendMe
		{
			get => _isSendMe;
			set => SetProperty(ref _isSendMe, value);
		}
		private bool _isExport = false;
		public bool IsExport
		{
			get => _isExport;
			set => SetProperty(ref _isExport, value);
		}
		private bool _isBasic;
		public bool IsBasic
		{
			get => _isBasic;
			set => SetProperty(ref _isBasic, value);
		}
		private bool _isSendToStudents;
		public bool IsSendToStudents
		{
			get => _isSendToStudents;
			set => SetProperty(ref _isSendToStudents, value);
		}


		public ExportViewModel(int currentGroupId, ref AppDbContext appDbContext)
		{
			CurrentMainGroupId = currentGroupId;
			Work = new UnitOfWork(new AppDbContext());
			Students = new ObservableCollection<Student>(Work.Students.GetAll().Where(x => x.MainGroup.Id == currentGroupId));
			Groups = new ObservableCollection<SubGroup>(Work.SubGroup.GetAll().Where(g => g.MainGroup.Id == currentGroupId));

			ExportCommand = new DelegateCommand(ExportData);

			IsAll = true;
			IsActivity = true;
			IsSelectActivity = false;
			IsSelectAttendance = false;
		}

		private async void ExportData()
		{
			IsExport = true;
			IExportService exportService = Services.ExportServiceFactory.GetExportService(IsExcel);

			exportService.AnonymizeData = IsAnonymized;
			exportService.SendToMe = IsSendMe;
			exportService.SendToStudents = IsSendToStudents;
			exportService.IsBasic = IsBasic;
			exportService.MainGroup = Work.MainGroup.Get(CurrentMainGroupId);
			exportService.Settings = Work.Settings.GetAll().FirstOrDefault();

			if (IsGroup)
			{
				exportService.Group = SelectedGroup;
			}
			else if (IsStudent)
			{
				exportService.Student = SelectedStudent;
			}
			try
			{
				exportService.Students = GetStudents();
				await System.Threading.Tasks.Task.Run(() =>
				{
					string pathToExport = string.Empty;
					if (IsActivity)
					{
						pathToExport = exportService.ExportActivity(ActivitiesForExport);
					}
					else if (IsAssessment)
					{
						pathToExport = exportService.ExportAssessment(GetAssessments());
					}
					else
					{
						pathToExport = exportService.ExportAttendances(AttendancesForExport);
					}

					System.Diagnostics.ProcessStartInfo startInfo = new(pathToExport)
					{
						UseShellExecute = true
					};
					System.Diagnostics.Process.Start(startInfo);

					IsExport = false;
				});
			}
			catch (Exception)
			{
				IsExport = false;
			}
		}
	}
}
