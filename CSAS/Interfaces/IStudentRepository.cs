using CSAS.Models;
using System.Collections.Generic;

namespace CSAS.Interfaces
{
	public interface IStudentRepository : IRepository<Student>
	{
		IEnumerable<Student> GetStudentsByGroup(MainGroup group);
		IEnumerable<Student> GetStudentsBySubGroup(SubGroup subGroup);

	}
}
