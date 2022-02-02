using System.IO;
using CSAS.Enums;
namespace CSAS.Services
{
	public class ExportExcelService : IExportService
	{
		readonly Logger _logger = new();
		public bool AnonymizeData { get; set; }
		public bool SendToMe { get; set; }
		public bool SendToStudents { get; set; }
		public SubGroup Group { get; set; }
		public Student Student { get; set; }
		public IList<Student> Students { get; set; }
		public bool IsBasic { get; set; }
		public MainGroup MainGroup { get; set; }
		public Settings Settings { get; set; }
		ExcelService ExcelService { get; set; } = new ExcelService();
		private string Type { get; set; }
		public string ExportActivity(IList<Activity> activities)
		{
			Type = "Aktivita";
			CreateDocument(Type);

			int rowIndex = 4;

			foreach (var student in Students)
			{
				InsertStudentData(student, rowIndex);
				rowIndex++;
				foreach (var activity in activities.Where(x => x.Student == student))
				{
					ExcelService.WriteRow(Type, rowIndex, false, string.Empty, activity.Name, activity.Deadline.ToShortDateString() + " " + activity.Deadline.ToShortTimeString(), activity.TotalPoints.ToString(), activity.EarnedPoints.ToString());
					rowIndex++;
					foreach (var task in activity.Tasks)
					{
						ExcelService.WriteRow(Type, rowIndex, false, string.Empty, string.Empty, task.Name, task.MaxPoints.ToString(), task.Points.ToString(), task.Comment);
						rowIndex++;
					}
				}
			}
			ExcelService.SaveFile();
			return CreateDocument(Type, true);
		}

		public string ExportAssessment(IList<FinalAssessment> finalAssessments)
		{
			Type = "Hodnotenie";
			CreateDocument(Type);

			int rowIndex = 4;

			AddHeader(rowIndex);
			rowIndex++;

			foreach (var student in Students)
			{
				var finalAssessment = finalAssessments.Where(x => x != null).FirstOrDefault(x => x.Student == student);
				if (finalAssessment == null)
				{
					continue;
				}
				if (AnonymizeData)
				{
					if (IsBasic)
					{
						ExcelService.WriteRow(Type, rowIndex, false, student.Isic,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
					else
					{
						ExcelService.WriteRow(Type, rowIndex, false, student.Isic, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
				else
				{
					if (IsBasic)
					{
						ExcelService.WriteRow(Type, rowIndex, false, student.FullName, student.Isic, student.SchoolEmail,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
					else
					{
						ExcelService.WriteRow(Type, rowIndex, false, student.FullName, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
				rowIndex++;
			}

			ExcelService.SaveFile();
			return CreateDocument(Type, true);
		}

		public string ExportAttendances(IList<Attendance> attendances)
		{
			Type = "Dochadzka";
			CreateDocument(Type);

			int rowIndex = 4;

			foreach (var student in Students)
			{
				InsertStudentData(student, rowIndex);
				rowIndex++;
				foreach (var attendance in attendances.Distinct())
				{
					var subAtt = attendance.SubAttendances.FirstOrDefault(x => x.Student == student);
					if (subAtt != null)
					{
						ExcelService.WriteRow(Type, rowIndex, false, string.Empty, attendance.Date.ToShortDateString() +" " + attendance.Date.ToShortTimeString(), EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceFormEnums>(attendance.Form.ToString()),
							EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceEnums>(subAtt.State.ToString()));
						rowIndex++;
					}
				}
			}

			ExcelService.SaveFile();

			return CreateDocument(Type, true);
		}

		private string CreateDocument(string type, bool isGetPath = false)
		{
			string path;
			try
			{
				if (Group != null)
				{
					path = Path.Combine(Group.PathToFolder, @$"`{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Group.Name}.xlsx");
				}
				else if (Student != null)
				{
					path = Path.Combine(Student.PathToFolder, @$"{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Student.FullName}.xlsx");
				}
				else
				{
					path = Path.Combine(MainGroup.PathToFolder, @$"{ DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) }_{ type}_{ MainGroup.Name}.xlsx");
				}
				if (!isGetPath)
				{
					ExcelService.CreateSpreadsheetWorkbook(path, type);

					ExcelService.WriteRow(Type, 1, false, "Dátum vytvorenia: ", DateTime.Now.ToShortDateString());
					ExcelService.WriteRow(Type, 2, false, "Vytvoril: ", $"{Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
				}
				return path;
			}
			catch(Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.InfoAsync(ex.StackTrace);
				return Path.Combine(MainGroup.PathToFolder, @$"{ DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) }_{ type}_{ MainGroup.Name}.xlsx");
			}
		}

		private void InsertStudentData(Student student, int rowIndex)
		{
			if (AnonymizeData)
			{
				if (IsBasic)
				{
					ExcelService.WriteRow(Type, rowIndex, false, student.Isic);
				}
				else
				{
					ExcelService.WriteRow(Type, rowIndex, false, student.Isic, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
			}
			else
			{
				if (IsBasic)
				{
					ExcelService.WriteRow(Type, rowIndex, false, student.FullName, student.Isic, student.SchoolEmail);
				}
				else
				{
					ExcelService.WriteRow(Type, rowIndex, false, student.FullName, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
			}
		}
		private void AddHeader(int rowIndex)
		{
			if (AnonymizeData)
			{
				if (IsBasic)
				{
					ExcelService.WriteRow(Type, rowIndex, false, "Isic", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					ExcelService.WriteRow(Type, rowIndex, false, "Isic", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
			else
			{
				if (IsBasic)
				{
					ExcelService.WriteRow(Type, rowIndex, false, "Meno", "Isic", "Školský email", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					ExcelService.WriteRow(Type, rowIndex, false, "Meno", "Isic", "Školský email", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
		}
	}
}
