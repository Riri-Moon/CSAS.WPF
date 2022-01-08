using System.ComponentModel;

namespace CSAS.Enums
{
	public class Enums
	{
		public enum AttendanceEnums
		{
			[Description("Neudelené")]
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
			[Description("Denná")]
			Daily,
			[Description("Externá")]
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
