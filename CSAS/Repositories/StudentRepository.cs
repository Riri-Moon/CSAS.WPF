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
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudentsSubByGroup(SubGroup subGroup)
        {
            throw new NotImplementedException();
        }

        public AppDbContext GetDbContext { get { return Context; } }
    }
}
