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
            [Description("Novo vytvorená dochádzka")]
            New,
            [Description("Neprítomný")]
            NotPresent,
            [Description("Prítomný")]
            IsPresent,
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

        public enum AttendanceFormEnums
        {
            [Description("Prednáška")]
            Lecture,
            [Description("Cvičenie")]
            Seminar,
        }

        public enum Grade
        {
            [Description("")]
            Empty,
            [Description("A")]
            A,
            [Description("B")]
            B,
            [Description("C")]
            C,
            [Description("D")]
            D,
            [Description("E")]
            E,
            [Description("Fx")]
            Fx,
        }
    }
}
