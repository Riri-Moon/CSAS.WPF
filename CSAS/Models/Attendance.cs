using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSAS.Enums;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
    [Table("Attendances")]
    public class Attendance : BaseModel
    {
        public virtual Student Student { get; set; }
        public virtual AttendanceEnums State { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
