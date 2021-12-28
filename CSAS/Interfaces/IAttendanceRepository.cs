using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        IEnumerable<Attendance> GetAttendanceByMainGroup(MainGroup group);
        IEnumerable<Attendance> GetAttendanceBySubGroup(SubGroup group);


    }
}
