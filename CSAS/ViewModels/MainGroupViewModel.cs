using CSAS.Views;
using Microsoft.Win32;
using System.Data;
using System.IO;
using Squirrel;
using System.Reflection;
using CSAS.Helpers;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
	public class MainGroupViewModel : BaseViewModelBindableBase
	{
		readonly Logger _logger = new();

		public DelegateCommand<object?> SelectMainGroupCommand { get; }
		public DelegateCommand CreateGroupCommand { get; }
		public DelegateCommand<string?> DeleteGroupCommand { get; }
		public DelegateCommand<bool> AcceptDelete { get; }
		public MainGroup? SelectedMainGroup { get; set; }
		private ObservableCollection<MainGroup> _mainGroups;
		private UpdateManager UpdateManager { get; set; }
		public ObservableCollection<MainGroup> MainGroups
		{
			get => _mainGroups;
			set => SetProperty(ref _mainGroups, value);
		}

		private bool _isMainGroupWindowVisible = true;
		public bool IsMainGroupWindowVisible
		{
			get => _isMainGroupWindowVisible;
			set => SetProperty(ref _isMainGroupWindowVisible, value);
		}
		private bool _isCreateMainGroup = false;
		public bool IsCreateMainGroup
		{
			get => _isCreateMainGroup;
			set => SetProperty(ref _isCreateMainGroup, value);
		}
		private bool _isSnackBarActive = false;
		public bool IsSnackBarActive
		{
			get => _isSnackBarActive;
			set => SetProperty(ref _isSnackBarActive, value);
		}

		private string _subject = string.Empty;
		public string Subject
		{
			get => _subject;
			set => SetProperty(ref _subject, value);
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
			Work = UoWSingleton.Instance;

			try
			{
//				AppHelper.LogUserDetails(Work);
//#if (!DEBUG)
//				IsLoading = true;
//			AppHelper.CheckForUpdates();
//				IsLoading = false;
//#endif
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.InfoAsync(ex.StackTrace);
			}

			MainGroups = new ObservableCollection<MainGroup>(Work.MainGroup.GetAll().ToList());
			SelectMainGroupCommand = new DelegateCommand<object?>(SelectGroup);
			CreateGroupCommand = new DelegateCommand(CreateGroup);
			DeleteGroupCommand = new DelegateCommand<string?>(DeleteGroup);
		}

		private async void CreateGroup()
		{
			ExcelService excelService = new();
			OpenFileDialog dialog = new();
			try
			{
				if (dialog.ShowDialog() == true)
				{
					IsLoading = true; 
					MainGroup mainGroup = new();
																
					await System.Threading.Tasks.Task.Run(() =>
					{						
						DataTable dt = ExcelService.GetDataTableFromExcelFile(dialog.FileName, "", 5);

						int nameIndex = ReturnIndexBasedOnName(dt, "Meno");
						int lastNameIndex = ReturnIndexBasedOnName(dt, "Priezvisko");
						int studyProgramIndex = ReturnIndexBasedOnName(dt, "Študijný program");
						int subGroupIndex = ReturnIndexBasedOnName(dt, "Krúžok");
						int isicIndex = ReturnIndexBasedOnName(dt, "Číslo karty");
						int yearIndex = ReturnIndexBasedOnName(dt, "Ročník");
						int schoolMailIndex = ReturnIndexBasedOnName(dt, "E-mail pridelený");
						int personalMailIndex = ReturnIndexBasedOnName(dt, "E-mail osobný");
						int gradeIndex = ReturnIndexBasedOnName(dt, "Hodnotenie"); 

						dt.BeginInit();
						dt.Rows[0].BeginEdit();
						dt.Rows[0].Delete();
						dt.Rows[0].EndEdit();
						dt.Rows[0].AcceptChanges();
						List<Student> students = new();

						string? name = dt.Rows[1].ItemArray[studyProgramIndex].ToString();

						var form = name.ElementAt(name.Length - 3);
						HashSet<string> subgroupNames = new();
						List<SubGroup> subGroups = new();

						mainGroup.Name = name;
						mainGroup.Subject = Subject;
						foreach (DataRow row in dt.Rows)
						{
							subgroupNames.Add(row.ItemArray[subGroupIndex].ToString());
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
						char[] grades = { 'A', 'B', 'C', 'D', 'E' };

						foreach (DataRow student in dt.Rows)
						{
							var newStudent = new Student()
							{
								Name = (string)student.ItemArray[nameIndex],
								LastName = (string)student.ItemArray[lastNameIndex],
								MainGroup = mainGroup,
								Form = 0,
								Email = student.ItemArray[personalMailIndex].ToString(),
								SchoolEmail = student.ItemArray[schoolMailIndex].ToString(),
								Isic = student.ItemArray[isicIndex].ToString().Remove(0, 5),
								Year = int.Parse((string)student.ItemArray[yearIndex]),
								SubGroup = subGroups.FirstOrDefault(x => x.Name == student.ItemArray[subGroupIndex].ToString()),
							};
							var grade = student.ItemArray[gradeIndex].ToString().First();

							if (grades.Contains(grade))
							{
								FinalAssessment finalAssessment = new();
								finalAssessment.Grade = Enums.EnumExtension.GetValueFromDescription<Grade>(grade.ToString());
								finalAssessment.IsNew = false;
								finalAssessment.IsSendAttendanceExport = false;
								finalAssessment.IsSendEmail = false;
								finalAssessment.IsSendExport = false;
								finalAssessment.Created = DateTime.Now;
								finalAssessment.Student = newStudent;
								Work.FinalAssessment.Add(finalAssessment);
							}
							students.Add(newStudent);
						}

						foreach (var subgrp in mainGroup.SubGroups)
						{
							subgrp.Students = new List<Student>();
							subgrp.Students.AddRange(students.Where(x => x.SubGroup.Name == subgrp.Name));
						}

						mainGroup.Form = form == 'D' ? "Denná" : "Externá";

						Work.MainGroup.Add(mainGroup);
						Work.Complete();
					});
					MainGroups.Add(mainGroup);

					CreateDirectories(mainGroup);
					IsLoading = false;
				}
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
				IsLoading = false;
			}
		}

		private int ReturnIndexBasedOnName(DataTable dt, string nameOfColumn)
		{
			if (nameOfColumn == "Meno")
			{
				return dt.Rows[0].ItemArray.ToList().IndexOf(nameOfColumn)+1;
			}

			return dt.Rows[0].ItemArray.ToList().IndexOf(nameOfColumn);
		}

		private async void DeleteGroup(string? id)
		{
			try
			{
				IsLoading = true;
				var group = MainGroups.FirstOrDefault(x => x.Id == id);
				await System.Threading.Tasks.Task.Run(() =>
				{
					var att = Work.Attendance.GetAttendanceByMainGroup(group);
					if (att.Any())
					{
						Work.Attendance.RemoveRange(att);
					}
					var studs = Work.Students.GetStudentsByGroup(group);
					var activities = new List<Models.Activity>();
					foreach (var stud in studs.ToList())
					{
						if (stud.ListOfActivities != null && stud.ListOfActivities.Any())
						activities.AddRange(stud.ListOfActivities.ToList());
					}

					foreach(var act in activities)
					{
						Work.Task.RemoveRange(act.Tasks);
					}

					Work.Activity.RemoveRange(activities);
					Work.Students.RemoveRange(Work.Students.GetStudentsByGroup(group));
					Work.SubGroup.RemoveRange(Work.SubGroup.GetSubGroupByMainGroup(group));

					var settings = Work.Settings.GetAll().FirstOrDefault(x => x.MainGroup == group);
					if (settings != null && settings.Id != null)
					{
						Work.Settings.Remove(settings);
					}
					Work.MainGroup.Remove(group);

					Work.Complete();
					IsLoading = false;
				});
				MainGroups.Remove(group);

			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
				IsLoading = false;
			}
		}

		private async void SelectGroup(object? id)
		{
			try
			{
				IsLoading = true;
				object[] param = id as object[];
				MainViewModel mainModel = null;
				await System.Threading.Tasks.Task.Run(() =>
			   {

				   mainModel = new MainViewModel((string)param[0], Assembly.GetExecutingAssembly().GetName().Version.ToString());
			   });

				MainWindow window = new()
				{
					DataContext = mainModel
				};

				window.Show();
				MainGroupView view = new();
				view = (MainGroupView)param[1];
				IsLoading = false;
				view.Close();
				IsMainGroupWindowVisible = false;
			}
			catch(Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
		}

		private void CreateDirectories(MainGroup mainGroup)
		{
			string mainPath = @"C:\CSAS\" + mainGroup.Name + "_" + mainGroup.Subject;
			if (!Directory.Exists(mainPath))
			{
				Directory.CreateDirectory(mainPath);
				mainGroup.PathToFolder = mainPath;
			}
			else
			{
				mainGroup.PathToFolder = mainPath;
			}

			foreach (var subgrp in mainGroup.SubGroups)
			{
				string subPath = $@"{mainPath}\{subgrp.Name}";

				if (!Directory.Exists(mainPath))
				{
					Directory.CreateDirectory(subPath);
					subgrp.PathToFolder = subPath;
				}
				else
				{
					subgrp.PathToFolder = subPath;
				}
				foreach (var stud in subgrp.Students)
				{
					string studentPath = $@"{mainPath}\{subgrp.Name}\{stud.Name} {stud.LastName}";
					Directory.CreateDirectory(studentPath);
					stud.PathToFolder = studentPath;
				}
			}

			Work.MainGroup.Update(mainGroup);
			Work.Complete();
		}
	}
}
