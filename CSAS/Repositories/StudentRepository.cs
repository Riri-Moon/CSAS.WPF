using CSAS.Interfaces;
using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
            
        }

        public IEnumerable<Student> GetStudentsByGroup(MainGroup group)
        {
            return Context.Students.Where(x => x.MainGroup == group);
        }

        public IEnumerable<Student> GetStudentsBySubGroup(SubGroup subGroup)
        {
            return Context.Students.Where(x => x.SubGroup == subGroup);
        }

        public AppDbContext GetDbContext { get { return Context; } }
    }
}
