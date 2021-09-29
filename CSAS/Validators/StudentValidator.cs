using CSAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Validators
{
    public class StudentValidator : BaseValidator
    {
        public static bool ValidateStudent(Student student)
        {
            bool isValid = false;
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
