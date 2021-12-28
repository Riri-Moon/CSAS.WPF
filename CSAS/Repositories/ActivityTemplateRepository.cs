using CSAS.Interfaces;
using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Repositories
{
    public class ActivityTemplateRepository : Repository<ActivityTemplate>, IActivityTemplateRepository
    {
        public ActivityTemplateRepository(AppDbContext context) : base(context)
        {
        }
    }
}
