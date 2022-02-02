using CSAS.Models;

namespace CSAS.Validators
{
	public class StudentValidator : BaseValidator
	{
		public static bool ValidateStudent(Student student)
		{
			return IsIsicValid(student.Isic) && IsEmailValid(student.SchoolEmail) && IsStringValid(student.Name) && student.SubGroup != null && student.Year.HasValue;
		}
	}
}
