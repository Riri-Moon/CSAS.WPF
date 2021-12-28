using CSAS.Interfaces;
using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Repositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext context) : base(context)
        {

        }    

        public AppDbContext GetDbContext { get { return Context; } }

        public IEnumerable<Attendance> GetAttendanceByMainGroup(MainGroup group)
        {
            return Context.Attendances.Where(x=>x.MainGroup == group);
        }

        public IEnumerable<Attendance> GetAttendanceBySubGroup(SubGroup group)
        {
            return Context.Attendances.Where(x => x.SubGroup == group);
        }
    }
}
