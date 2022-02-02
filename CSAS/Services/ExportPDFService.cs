using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using CSAS.Enums;
namespace CSAS.Services
{
	public class ExportPDFService : IExportService
	{
		public bool AnonymizeData { get; set; }
		public bool SendToMe { get; set; }
		public bool SendToStudents { get; set; }
		public SubGroup Group { get; set; }
		public Student Student { get; set; }
		public MainGroup MainGroup { get; set; }
		public IList<Student> Students { get; set; }
		public bool IsBasic { get; set; }
		public Settings Settings { get; set; }
		private string Type { get; set; }
		private Document Document { get; set; }
		private string PathToFile { get; set; }
		private readonly string FONT = @"Fonts\ARIAL.TTF";
		public string ExportActivity(IList<Activity> activities)
		{
			Type = "Aktivita";
			CreateDocument(GetPath(Type));
			float[] x = { 150, 100, 100, 50, 50, 400 };

			Table table = new(UnitValue.CreatePointArray(x));

			foreach (var student in Students)
			{
				if (AnonymizeData)
				{
					table = IsBasic
						? InsertCell(table, 6, student.Isic)
						: InsertCell(table, 6, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
				else
				{
					table = IsBasic
						? InsertCell(table, 6, student.FullName, student.Isic)
						: InsertCell(table, 6, student.FullName, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), Enums.EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));
				}
				foreach (var activity in activities.Where(x => x.Student == student))
				{
					table = InsertCell(table, 6, string.Empty, activity.Name, activity.Deadline.ToShortDateString() + " " + activity.Deadline.ToShortTimeString(), activity.TotalPoints.ToString(), activity.EarnedPoints.ToString());

					foreach (var task in activity.Tasks)
					{
						table = InsertCell(table, 6, string.Empty, string.Empty, task.Name, task.MaxPoints.ToString(), task.Points.ToString(), task.Comment);
					}
				}
			}

			Document.Add(table);
			Document.Close();

			return PathToFile;
		}

		public string ExportAssessment(IList<FinalAssessment> finalAssessments)
		{
			Type = "Hodnotenie";

			CreateDocument(GetPath(Type));
			Document.SetFontSize(9);

			Table table = null;			

			table = AddHeader(table);

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
						table = InsertCell(table, 8, student.Isic,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
					else
					{
						table = InsertCell(table, 11, student.Isic, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
				else
				{
					if (IsBasic)
					{
						table = InsertCell(table, 9, student.FullName, student.Isic, student.SchoolEmail,
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
					else
					{
						table = InsertCell(table, 12, student.FullName, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(), student.Form.ToString(),
							finalAssessment.Created.ToShortDateString(), student.TotalPoints.ToString(), student.MissedLectures.ToString(), student.MissedSeminars.ToString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.Grade>(finalAssessment.Grade.ToString()), finalAssessment.Comment);
					}
				}
			}

			Document.Add(table);
			Document.Close();

			return PathToFile;
		}

		public string ExportAttendances(IList<Attendance> attendances)
		{
			Type = "Dochadzka";
			CreateDocument(GetPath(Type));
			Table table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth();

			foreach (var student in Students)
			{
				table = AnonymizeData
					? IsBasic
						? InsertCell(table,6,student.Isic)
						: InsertCell(table,
										   6,
										   student.Isic,
										   student.SubGroup.Name,
										   student.Year.ToString(),
										   EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()))
					: IsBasic
						? InsertCell(table, 6, student.FullName, student.Isic, student.SchoolEmail)
						: InsertCell(table, 6, student.FullName, student.Isic, student.SchoolEmail, student.SubGroup.Name, student.Year.ToString(),
						EnumExtension.GetDescriptionValue<Enums.Enums.FormEnums>(student.Form.ToString()));

				foreach (var attendance in attendances.Distinct())
				{
					var subAtt = attendance.SubAttendances.FirstOrDefault(x => x.Student == student);
					if (subAtt != null)
					{
						table = InsertCell(table, 6, string.Empty, attendance.Date.ToShortDateString() + " " + attendance.Date.ToShortTimeString(),
							EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceFormEnums>(attendance.Form.ToString()),
							EnumExtension.GetDescriptionValue<Enums.Enums.AttendanceEnums>(subAtt.State.ToString()));
					}
				}
			}

			Document.Add(table);
			Document.Close();

			return PathToFile;
		}

		private string CreateDocument(string path)
		{
			PdfDocument pdfDoc = new(new PdfWriter(path));
			Document doc = new(pdfDoc, PageSize.A4.Rotate());

			Document = doc;
			PdfFont font = PdfFontFactory.CreateFont(FONT);
			Document.SetFont(font);
			Paragraph para = new($"Vytvorené dňa: {DateTime.Now.ToShortDateString()}");
			Paragraph para1 = new($"Vytvoril: {Settings.Title} {Settings.Name} {Settings.TitleAfterName}");

			Document.Add(para);
			Document.Add(para1);
			return PathToFile = path;
		}

		private string GetPath(string type)
		{
			string path;
			if (Group != null)
			{
				path = System.IO.Path.Combine(Group.PathToFolder, @$"{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Group.Name}.pdf");
			}
			else if (Student != null)
			{
				path = System.IO.Path.Combine(Student.PathToFolder, @$"{DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.InvariantCulture)}_{type}_{Student.FullName}.pdf");
			}
			else
			{
				path = System.IO.Path.Combine(MainGroup.PathToFolder, @$"{ DateTime.Now.ToString("ddMMyyHHmmss", System.Globalization.CultureInfo.CurrentCulture) }_{ type}_{ MainGroup.Name}.pdf");
			}

			return path;
		}

		private static Table InsertCell(Table table, int count, params string[] values)
		{
			for (int i = 0; i < count; i++)
			{
				if (i < values.Length && values[i] != null)
				{
					table.AddCell(values[i]);
				}
				else
				{
					table.AddCell(string.Empty);
				}
			}
			return table;
		}

		private Table AddHeader(Table table)
		{
			if (AnonymizeData)
			{
				if (IsBasic)
				{
					table = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
					table = InsertCell(table, 8, "Isic", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					table = new Table(UnitValue.CreatePercentArray(9)).UseAllAvailableWidth();
					table = InsertCell(table, 11, "Isic", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
			else
			{
				if (IsBasic)
				{
					table = new Table(UnitValue.CreatePercentArray(9)).UseAllAvailableWidth();
					table = InsertCell(table, 9, "Meno", "Isic", "Školský email", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
				else
				{
					table = new Table(UnitValue.CreatePercentArray(12)).UseAllAvailableWidth();
					table = InsertCell(table, 12, "Meno", "Isic", "Školský email", "Skupina", "Ročník", "Forma štúdia", "Dátum vytvorenia", "Získané body", "Vymeškané prednášky", "Vymeškané cvičenia", "Známka", "Komentár");
				}
			}
			return table;
		}
	}
}
