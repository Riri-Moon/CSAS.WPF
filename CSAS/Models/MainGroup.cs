using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class MainGroup : BaseWithNameModel
    {
        public virtual string Form {  get; set; }
        public virtual List<SubGroup> SubGroups {  get; set; }

        public MainGroup()
        {
            
        }
    }
}
