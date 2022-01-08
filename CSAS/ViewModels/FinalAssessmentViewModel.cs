using System.Net.Mail;
using System.Text;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
	public class FinalAssessmentViewModel : BaseViewModelBindableBase
	{
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
					var assessment = _work.FinalAssessment.GetAll().FirstOrDefault(x => x.Student.Id == value.Id);
					if (assessment != null)
						FinalAssessment = assessment;
					else
					{
						FinalAssessment = new FinalAssessment
						{
							Grade = GetGrade(value.TotalPoints.Value)
						};
					}
				}
				SetProperty(ref _student, value);
			}
		}
		private FinalAssessment _finalAssessment;
		public FinalAssessment FinalAssessment
		{
			get => _finalAssessment;
			set => SetProperty(ref _finalAssessment, value);
		}

		private new UnitOfWork _work;

		public FinalAssessmentViewModel(int currentGroupId, ref AppDbContext context)
		{
			_work = new UnitOfWork(context);
			CurrentMainGroupId = currentGroupId;
			RefreshCommand = new DelegateCommand(Reload);
			Students = new ObservableCollection<Student>(_work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId)));
			SaveCommand = new DelegateCommand(SaveFinalAssessment);
		}

		private void Reload()
		{
			_work = new UnitOfWork(new AppDbContext());
			Students = new ObservableCollection<Student>(_work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId)));
		}

		private Grade GetGrade(double pts)
		{
			var grades = _work.Settings.GetAll().FirstOrDefault();

			if (pts >= grades.A)
			{
				return Grade.A;
			}
			if (pts >= grades.B)
			{
				return Grade.B;
			}
			if (pts >= grades.C)
			{
				return Grade.C;
			}
			if (pts >= grades.D)
			{
				return Grade.D;
			}
			if (pts >= grades.E)
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
			OutlookService outlookService = new();
			string space = "<br/><br/>";

			if (FinalAssessment.IsNew)
			{
				FinalAssessment.Student = SelectedStudent;
				FinalAssessment.IsNew = false;
				FinalAssessment.Created = DateTime.Now;
				_work.FinalAssessment.Add(FinalAssessment);
			}
			else
			{
				_work.FinalAssessment.Update(FinalAssessment);
			}
			_work.Complete();

			if (FinalAssessment.IsSendEmail)
			{
				StringBuilder builder = new($"Dobrý deň, {space} Vaše hodnotenie z predmetu {FinalAssessment.Student.MainGroup.Subject} je {Enums.EnumExtension.GetDescription(FinalAssessment.Grade)}.");
				List<string> paths = new();
				var sett = _work.Settings.GetAll().FirstOrDefault();
				MailAddressCollection mails = new()
				{
					FinalAssessment.Student.Email
				};
				if (FinalAssessment.IsSendExport)
				{
					ExportExcelService service = new()
					{
						AnonymizeData = false,
						IsBasic = true,
						SendToStudents = true,
						Student = FinalAssessment.Student,
						Settings = _work.Settings.GetAll().FirstOrDefault(),
					};
					var pathToActivity = service.ExportActivity(_work.Activity.GetAll().Where(x => x.Student == FinalAssessment.Student).ToList());
					builder.Append($"{space} V prílohe nájdete výpis z aktivít");
					paths.Add(pathToActivity);

					if (FinalAssessment.IsSendAttendanceExport)
					{
						var pathToAttendances = service.ExportAttendances(_work.Attendance.GetAll().Where(x => x.MainGroup == FinalAssessment.Student.MainGroup).ToList());
						builder.Append($"a dochádzky.");
						paths.Add(pathToAttendances);


					}
					outlookService.SendEmail(new MailAddress(sett.Email), $"Konečné hodnotenie z predmetu {FinalAssessment.Student.MainGroup.Subject}", mails, null, builder.ToString(), paths, false);

				}
				builder.Append($"S pozdravom {sett.Title} {sett.Name} {sett.TitleAfterName}");
				outlookService.SendEmail(new MailAddress(sett.Email), $"Konečné hodnotenie z predmetu {FinalAssessment.Student.MainGroup.Subject}", mails, null, builder.ToString(),null,false);
			}
		}
	}
}
