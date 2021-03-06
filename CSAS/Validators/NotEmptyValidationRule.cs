using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;

namespace CSAS.Validators
{

	public class NotEmptyValidationRule : ValidationRule
	{

		public event PropertyChangedEventHandler? PropertyChanged;

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return string.IsNullOrWhiteSpace((value ?? "").ToString())
				? new ValidationResult(false, "Field is required.")
				: ValidationResult.ValidResult;
		}
	}
}
