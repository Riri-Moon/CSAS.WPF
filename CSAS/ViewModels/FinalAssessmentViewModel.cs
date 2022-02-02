using CSAS.Helpers;
using IBM.Tools.Common.Helper.Logger;
using System.Net.Mail;
using System.Text;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
	public class FinalAssessmentViewModel : BaseViewModelBindableBase
	{
		readonly Logger logger = new();
		public DelegateCommand RefreshCommand { get; }
		public DelegateCommand SaveCommand { get; }

		private ObservableCollection<Student> _students;
		public ObservableCollection<Student> Students
		{
			get => _students;
			set => SetProperty(ref _students, value);
		}

		public static Array Grades
		{
			get
			{
				List<string> arr = new();

				foreach (var grade in Enum.GetValues(typeof(Enums.Enums.Grade)))
				{
					arr.Add(Enums.EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(grade.ToString()));
				}
				return arr.ToArray();
			}
		}
		private Student _student;
		public Student SelectedStudent
		{

			get => _student;
			set
			{
				if (value != null)
				{
					if (value.FinalAssessment == null)
					{
						value.FinalAssessment = new FinalAssessment
						{
							Grade = GetGrade(value.TotalPoints.Value),
							IsNew = true,
							Student = value,
							Created= DateTime.Now,
						};
					}
				}
				SetProperty(ref _student, value);
			}
		}

		public FinalAssessmentViewModel(string currentGroupId, ref AppDbContext context)
		{
			Work = new UnitOfWork(context);
			CurrentMainGroupId = currentGroupId;
			RefreshCommand = new DelegateCommand(Reload);
			Students = new ObservableCollection<Student>(Work.Students.GetStudentsByGroup(Work.MainGroup.Get(CurrentMainGroupId)));
			SaveCommand = new DelegateCommand(SaveFinalAssessment);
		}

		private void Reload()
		{
			var mainGroup = Work.MainGroup.Get(CurrentMainGroupId);
			Work = new UnitOfWork(new AppDbContext());
			Students = new ObservableCollection<Student>(Work.Students.GetStudentsByGroup(mainGroup));
			var finAssessments = Work.FinalAssessment.GetAll();
			foreach (var stud in Students)
			{
				var finass = finAssessments.FirstOrDefault(x => x.Student == stud);
				if (finass != null)
					stud.FinalAssessment = finass;
			}
		}

		private Grade GetGrade(double pts)
		{
			var grades = Work.Settings.GetAll().FirstOrDefault(x=>x.MainGroup.Id == CurrentMainGroupId);
			var percentage = grades.MaxPoints.Value / 100;

			if (pts >= grades.A * percentage)
			{
				return Grade.A;
			}
			if (pts >= grades.B * percentage)
			{
				return Grade.B;
			}
			if (pts >= grades.C * percentage)
			{
				return Grade.C;
			}
			if (pts >= grades.D * percentage)
			{
				return Grade.D;
			}
			if (pts >= grades.E * percentage)
			{
				return Grade.E;
			}
			else
			{
				return Grade.Fx;
			}
		}

		private void SaveFinalAssessment()
		{
			string space = "<br/><br/>";
			if (SelectedStudent == null)
			{
				MessageBoxHelper.Show("", "Nie je vybraný žiadny študent", true);
				return;
			}
			if (SelectedStudent.FinalAssessment.IsNew)
			{
				SelectedStudent.FinalAssessment.IsNew = false;
				SelectedStudent.FinalAssessment.Created = DateTime.Now;
				if (string.IsNullOrEmpty(SelectedStudent.FinalAssessment.Id))
				{
					Work.FinalAssessment.Add(SelectedStudent.FinalAssessment);
				}
				else
				{
					Work.FinalAssessment.Update(SelectedStudent.FinalAssessment);
				}
			}
			else
			{
				Work.FinalAssessment.Update(SelectedStudent.FinalAssessment);

			}

			Work.Complete();

			if (SelectedStudent.FinalAssessment.IsSendEmail)
			{
				OutlookService outlookService = new();
				StringBuilder builder = new($"Dobrý deň, {space} Vaše hodnotenie z predmetu {SelectedStudent.FinalAssessment.Student.MainGroup.Subject} je {Enums.EnumExtension.GetDescription(SelectedStudent.FinalAssessment.Grade)}.");
				List<string> paths = new();
				try
				{
					var sett = Work.Settings.GetAll().FirstOrDefault();
					MailAddressCollection mails = new()
					{
						SelectedStudent.Email
					};

					if (SelectedStudent.FinalAssessment.IsSendExport)
					{
						List<Student> students = new();
						students.Add(SelectedStudent);
						ExportExcelService service = new()
						{
							AnonymizeData = false,
							IsBasic = true,
							SendToStudents = true,
							Student = SelectedStudent,
							Students = students,
							Settings = Work.Settings.GetAll().FirstOrDefault(x=>x.MainGroup==SelectedStudent.MainGroup),
						};
						var pathToActivity = service.ExportActivity(Work.Activity.GetAll().Where(x => x.Student == SelectedStudent).ToList());
						builder.Append($"{space} V prílohe nájdete výpis z aktivít");
						paths.Add(pathToActivity);

						if (SelectedStudent.FinalAssessment.IsSendAttendanceExport)
						{
							var pathToAttendances = service.ExportAttendances(Work.Attendance.GetAll().Where(x => x.MainGroup == SelectedStudent.MainGroup).ToList());
							builder.Append($"a dochádzky.");
							paths.Add(pathToAttendances);
						}
						builder.Append($"S pozdravom {sett.Title} {sett.Name} {sett.TitleAfterName}");

						outlookService = new();
						outlookService.SendEmail($"Konečné hodnotenie z predmetu {SelectedStudent.MainGroup.Subject}", mails, null, builder.ToString(), paths, false);
					}
					else
					{
						builder.Append($"S pozdravom {sett.Title} {sett.Name} {sett.TitleAfterName}");
						outlookService.SendEmail( $"Konečné hodnotenie z predmetu {SelectedStudent.MainGroup.Subject}", mails, null, builder.ToString(), null, false);
					}
				}
				catch (Exception ex)
				{
					logger.ErrorAsync(ex.Message);
					logger.ErrorAsync(ex.StackTrace);
				}
			}
		}
	}
}
