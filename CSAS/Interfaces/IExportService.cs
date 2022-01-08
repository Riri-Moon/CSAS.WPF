using CSAS.Models;
using System.Collections.Generic;

namespace CSAS.Interfaces
{
	public interface IExportService
	{
		bool AnonymizeData { get; set; }
		bool SendToMe { get; set; }
		bool SendToStudents { get; set; }
		SubGroup Group { get; set; }
		Student Student { get; set; }
		MainGroup MainGroup { get; set; }
		Settings Settings { get; set; }
		IList<Student> Students { get; set; }
		bool IsBasic { get; set; }

		string ExportActivity(IList<Activity> activities);
		string ExportAttendances(IList<Attendance> attendances);
		string ExportAssessment(IList<FinalAssessment> finalAssessments);
	}
}
