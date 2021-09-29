using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class SubGroup : BaseWithNameModel
    {
        public virtual MainGroup MainGroup {  get; set; }
        public virtual List<Student> Students {  get; set; }
    }
}
