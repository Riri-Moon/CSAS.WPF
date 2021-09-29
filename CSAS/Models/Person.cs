using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class Person : BaseWithNameModel
    {
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
    }
}
