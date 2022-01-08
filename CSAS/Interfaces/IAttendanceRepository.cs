using CSAS.Models;
using System.Collections.Generic;

namespace CSAS.Interfaces
{
	public interface IAttendanceRepository : IRepository<Attendance>
	{
		IEnumerable<Attendance> GetAttendanceByMainGroup(MainGroup group);
		IEnumerable<Attendance> GetAttendanceBySubGroup(SubGroup group);


	}
}
