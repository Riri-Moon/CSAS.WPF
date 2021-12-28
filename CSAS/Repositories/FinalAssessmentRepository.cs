using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace CSAS.Repositories
{
    public class FinalAssessmentRepository : Repository<FinalAssessment>, IFinalAssessmentRepository
    {
        public FinalAssessmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
