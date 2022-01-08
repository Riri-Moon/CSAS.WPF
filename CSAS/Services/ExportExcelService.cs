using System.IO;
using CSAS.Enums;
namespace CSAS.Services
{
	public class ExportExcelService : IExportService
	{
		public bool AnonymizeData { get; set; }
		public bool SendToMe { get; set; }
		public bool SendToStudents { get; set; }
		public SubGroup Group { get; set; }
		public Student Student { get; set; }
		public IList<Student> Students { get; set; }
		public bool IsBasic { get; set; }
		public MainGroup MainGroup { get; set; }
		public Settings Settings { get; set; }
		ExcelService excelService { get; set; } = new ExcelService();
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
					excelService.WriteRow(Type, rowIndex, false, string.Empty, activity.Name, activity.Deadline.ToShortDateString(), activity.TotalPoints.ToString(), activity.EarnedPoints.ToString());
					rowIndex++;
					foreach (var task in activity.Tasks)
					{
						excelService.WriteRow(Type, rowIndex, false, string.Empty, string.Empty, task.Name, task.MaxPoints.ToString(), task.Points.ToString(), task.Comment);
						rowIndex++;
					}
				}
			}
			excelService.SaveFile();
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
						excelService.WriteRow(Type, rowIndex, false, student.Isic, student.SchoolEmail,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
					else
					{
						excelService.WriteRow(Type, rowIndex, false, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
				else
				{
					if (IsBasic)
					{
						excelService.WriteRow(Type, rowIndex, false, student.Name, student.Isic, student.SchoolEmail,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);

					}
					else
					{
						excelService.WriteRow(Type, rowIndex, false, student.Name, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
				rowIndex++;
			}

			excelService.SaveFile();
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
				foreach (var attendance in attendances)
				{
					var subAtt = attendance.SubAttendances.FirstOrDefault(x => x.Student == student);
					if (subAtt != null)
					{
						excelService.WriteRow(Type, rowIndex, false, string.Empty, attendance.Date.ToShortDateString(), EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceFormEnums>(attendance.Form.ToString()),
							EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceEnums>(subAtt.State.ToString()));
						rowIndex++;
					}
				}
			}

			excelService.SaveFile();

			return CreateDocument(Type, true);
		}

		private string CreateDocument(string type, bool isGetPath = false)
		{
			string path;
			if (Group != null)
			{
				path = Path.Combine(Group.PathToFolder, @$"`{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Group.Name}.xlsx");
			}
			else if (Student != null)
			{
				path = Path.Combine(Student.PathToFolder, @$"{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Student.Name}.xlsx");
			}
			else
			{
				path = Path.Combine(MainGroup.PathToFolder, @$"{ DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) }_{ type}_{ MainGroup.Name}.xlsx");
			}
			if (!isGetPath)
			{
				excelService.CreateSpreadsheetWorkbook(path, type);

				excelService.WriteRow(Type, 1, false, "Dátum vytvorenia: ", DateTime.Now.ToShortDateString());
				excelService.WriteRow(Type, 2, false, "Vytvoril: ", $"{Settings.Title} {Settings.Name} {Settings.TitleAfterName}");
			}
			return path;
		}

		private void InsertStudentData(Student student, int rowIndex)
		{
			if (AnonymizeData)
			{
				if (IsBasic)
				{
					excelService.WriteRow(Type, rowIndex, false, student.Isic, student.SchoolEmail);
				}
				else
				{
					excelService.WriteRow(Type, rowIndex, false, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
			}
			else
			{
				if (IsBasic)
				{
					excelService.WriteRow(Type, rowIndex, false, student.Name, student.Isic, student.SchoolEmail);
				}
				else
				{
					excelService.WriteRow(Type, rowIndex, false, student.Name, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
			}
		}
		private void AddHeader(int rowIndex)
		{
			if (AnonymizeData)
			{
				if (IsBasic)
				{
					excelService.WriteRow(Type, rowIndex, false, "Isic", "Školský email", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					excelService.WriteRow(Type, rowIndex, false, "Isic", "Školský email", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
			else
			{
				if (IsBasic)
				{
					excelService.WriteRow(Type, rowIndex, false, "Meno", "Isic", "Školský email", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					excelService.WriteRow(Type, rowIndex, false, "Meno", "Isic", "Školský email", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
		}
	}
}
