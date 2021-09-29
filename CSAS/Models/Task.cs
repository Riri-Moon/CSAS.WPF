using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class Task
    {
        public int Id {  get; set; }
        public virtual Activity Activity { get; set; }
        public virtual string Name {  get; set; }
        public virtual int? MaxPoints {  get; set; }
        public virtual int? Points {  get; set; }
        public virtual string? Comment { get; set; }
    }
}
