using System;
using System.ComponentModel.DataAnnotations;

namespace CSAS.Validators
{
    public class BaseValidator 
    {
        public static bool IsEmailValid(string email)
        {
            if( new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            else { return false; }
        }

        public static bool IsIsicValid(string isic)
        {
            return isic.Length == 17;
        }

        public static bool IsStringValid(string someString)
        {
            if( string.IsNullOrEmpty(someString) || string.IsNullOrWhiteSpace(someString))
            {
                return false;
            }
            else {  return true; }
        }
    }
}
