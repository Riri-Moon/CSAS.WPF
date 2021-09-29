using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSAS.Models
{
    public class Activity : BaseWithNameModel
    {
        public virtual List<Task> Tasks {  get; set; }
        public virtual Student Student {  get; set; }
        public virtual int TotalPoints { get; set; }
    }
}
