using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class BaseWithNameModel : BaseModel
    {
        public virtual string Name {  get; set; }
    }
}
