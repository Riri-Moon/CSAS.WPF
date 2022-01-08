using CSAS.Models;

namespace CSAS.Validators
{
	public class StudentValidator : BaseValidator
	{
		public static bool ValidateStudent(Student student)
		{
			bool isValid;
			if (!IsStringValid(student.Name) && !IsEmailValid(student.SchoolEmail))
			{
				isValid = false;
				return false;
			}
			else
			{
				isValid = true;
			}

			if (IsStringValid(student.Email))
			{
				if (isValid && IsEmailValid(student.Email))
				{
					isValid = true;
				}
				else
				{
					return false;
				}
			}

			return isValid;
		}
	}
}
