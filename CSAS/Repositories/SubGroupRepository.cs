using CSAS.Interfaces;
using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Repositories
{
    public class SubGroupRepository : Repository<SubGroup>, ISubGroupRepository
    {
        public SubGroupRepository(AppDbContext context) : base(context)
        {

        }    

        public AppDbContext GetDbContext { get { return Context; } }
    }
}
