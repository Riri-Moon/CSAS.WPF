using CSAS.Views;
using Microsoft.Win32;
using System.Data;
using System.IO;
using Squirrel;
using IBM.Tools.Common.Helper;
using IBM.Tools.Common.Helper.Logger;
using System.Reflection;

namespace CSAS.ViewModels
{
	public class MainGroupViewModel : BaseViewModelBindableBase
	{
		Logger _logger = new Logger();

		public DelegateCommand<object?> SelectMainGroupCommand { get; }
		public DelegateCommand CreateGroupCommand { get; }
		public DelegateCommand<int?> DeleteGroupCommand { get; }
		public DelegateCommand<bool> AcceptDelete { get; }

		public MainGroup? SelectedMainGroup { get; set; }
		private ObservableCollection<MainGroup> _mainGroups;
		private UpdateManager UpdateManager { get; set; }
		public ObservableCollection<MainGroup> MainGroups
		{
			get { return _mainGroups; }
			set
			{
				SetProperty(ref _mainGroups, value);
			}
		}

		private bool _isMainGroupWindowVisible = true;
		public bool IsMainGroupWindowVisible
		{
			get { return _isMainGroupWindowVisible; }
			set
			{
				SetProperty(ref _isMainGroupWindowVisible, value);
			}
		}
		private bool _isCreateMainGroup = false;
		public bool IsCreateMainGroup
		{
			get { return _isCreateMainGroup; }
			set
			{
				SetProperty(ref _isCreateMainGroup, value);
			}
		}
		private bool _isSnackBarActive = false;
		public bool IsSnackBarActive
		{
			get { return _isSnackBarActive; }
			set
			{
				SetProperty(ref _isSnackBarActive, value);
			}
		}
		private string _subject = string.Empty;
		public string Subject
		{
			get { return _subject; }
			set
			{
				SetProperty(ref _subject, value);
			}
		}
		public MainGroupViewModel()
		{
			_logger.SetConfiguration(LogTargets.SingleFile, new LogConfiguration
			{
				LogToLevel = LogType.Info,
				MediaName = @"C:\CSAS",
				MediaRecord = "Log_CSAS_",
				MediaSize = 500,
				UseUtcTime = true
			});
			_logger.InfoAsync("Log file created");
#if(RELEASE)
			CheckForUpdates();
#endif
			_work = new UnitOfWork(new AppDbContext());
			MainGroups = new ObservableCollection<MainGroup>(_work.MainGroup.GetAll().ToList());
			SelectMainGroupCommand = new DelegateCommand<object?>(SelectGroup);
			CreateGroupCommand = new DelegateCommand(CreateGroup);
			DeleteGroupCommand = new DelegateCommand<int?>(DeleteGroup);
		}

		private async void CheckForUpdates()
		{
			try
			{
				using (UpdateManager = await UpdateManager.GitHubUpdateManager(@"https://github.com/Riri-Moon/CSAS.WPF"))
				{

					var isUpdate = await UpdateManager.CheckForUpdate();

					if (isUpdate != null && isUpdate.ReleasesToApply != null && isUpdate.ReleasesToApply.Any())
					{
						await UpdateManager.UpdateApp();
					}
				}
			}
			catch(Exception ex)
			{
				_logger.InfoAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
		}

		private void CreateGroup()
		{
			ExcelService excelService = new();
			OpenFileDialog dialog = new();
			if (dialog.ShowDialog() == true)
			{
				excelService.OpenDocument(dialog.FileName);
				for (int i = 0; i < 5; i++)
				{
					excelService.RemoveRow();
				}

				excelService.SaveFile();

				MainGroup mainGroup = new();

				DataTable dt = ExcelService.GetDataTableFromExcelFile(dialog.FileName);
				dt.BeginInit();
				dt.Rows[0].BeginEdit();
				dt.Rows[0].Delete();
				dt.Rows[0].EndEdit();
				dt.Rows[0].AcceptChanges();
				List<Student> students = new();


				string? name = dt.Rows[1].ItemArray[3].ToString();
				var form = name.ElementAt(name.Length - 3);
				HashSet<string> subgroupNames = new();
				List<SubGroup> subGroups = new();

				mainGroup.Name = name;
				mainGroup.Subject = Subject;
				foreach (DataRow row in dt.Rows)
				{
					subgroupNames.Add(row.ItemArray[5].ToString());
				}

				foreach (var group in subgroupNames)
				{
					subGroups.Add(new SubGroup()
					{
						MainGroup = mainGroup,
						Name = group,
					});
				}
				mainGroup.SubGroups = new List<SubGroup>();
				mainGroup.SubGroups.AddRange(subGroups);

				foreach (DataRow student in dt.Rows)
				{
					students.Add(new Student()
					{
						Name = student.ItemArray[1] + " " + student.ItemArray[2],
						MainGroup = mainGroup,
						Form = 0,
						Email = student.ItemArray[30].ToString(),
						SchoolEmail = student.ItemArray[31].ToString(),
						Isic = student.ItemArray[19].ToString().Remove(0, 5),
						Year = int.Parse((string)student.ItemArray[21]),
						SubGroup = subGroups.FirstOrDefault(x => x.Name == student.ItemArray[5].ToString())

					});
				}

				foreach (var subgrp in mainGroup.SubGroups)
				{
					subgrp.Students = new List<Student>();
					subgrp.Students.AddRange(students.Where(x => x.SubGroup.Name == subgrp.Name));
				}

				mainGroup.Form = form == 'D' ? "Denná" : "Externá";

				_work.MainGroup.Add(mainGroup);
				var result = _work.Complete();
				MainGroups.Add(mainGroup);

				CreateDirectories(mainGroup);

			}
		}
		private void DeleteGroup(int? id)
		{
			var group = MainGroups.FirstOrDefault(x => x.Id == id.Value);
			_work.Students.RemoveRange(_work.Students.Find(x => x.MainGroup == group));
			_work.SubGroup.RemoveRange(_work.SubGroup.Find(x => x.MainGroup == group));
			_work.MainGroup.Remove(group);

			_work.Complete();
			MainGroups.Remove(group);
		}
		private void SelectGroup(object? id)
		{
			object[] param = id as object[];
#if (!DEBUG)
			MainWindow window = new()
			{
				DataContext = new MainViewModel((int)param[0],UpdateManager.CurrentlyInstalledVersion().ToString())
			};
#else
			MainWindow window = new()
			{
				DataContext = new MainViewModel((int)param[0], Assembly.GetExecutingAssembly().GetName().Version.ToString())
			};
#endif

			window.Show();
			MainGroupView view = new();
			view = (MainGroupView)param[1];
			view.Close();
			IsMainGroupWindowVisible = false;
		}

		private void CreateDirectories(MainGroup mainGroup)
		{
			string mainPath = @"C:\CSAS\" + mainGroup.Name + "_" + mainGroup.Subject;
			if (!Directory.Exists(mainPath))
			{
				Directory.CreateDirectory(mainPath);
				mainGroup.PathToFolder = mainPath;
			}

			foreach (var subgrp in mainGroup.SubGroups)
			{
				if (!Directory.Exists(mainPath))
				{
					string subPath = $@"{mainPath}\{subgrp.Name}";
					Directory.CreateDirectory(subPath);
					subgrp.PathToFolder = subPath;
				}
				foreach (var stud in subgrp.Students)
				{
					string studentPath = $@"{mainPath}\{subgrp.Name}\{stud.Name}";
					Directory.CreateDirectory(studentPath);
					stud.PathToFolder = studentPath;
				}
			}

			_work.MainGroup.Update(mainGroup);
			_work.Complete();
		}
	}
}
