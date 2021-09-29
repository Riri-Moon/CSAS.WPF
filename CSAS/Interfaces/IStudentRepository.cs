using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        IEnumerable<Student> GetStudentsByGroup(MainGroup group);
        IEnumerable<Student> GetStudentsSubByGroup(SubGroup subGroup);

    }
}
