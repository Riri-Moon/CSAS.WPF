using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Enums
{
    public class Enums
    {
        public enum AttendanceEnums
        {
            [Description("Neprítomný")]
            False,
            [Description("Prítomný")]
            True,
            [Description("Nahradené")]
            Subbed,
            [Description("Ospravedlnené")]
            Excused,
        }

        public enum FormEnums
        {
            [Description("Denná forma")]
            Daily,
            [Description("Externá forma")]
            Extern,
        }
    }
}
