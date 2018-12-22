using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace DebtsAPI.Dtos.ValidationAttributes
{
    class ValidEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;

            if (value == null)
            {
                return ValidationResult.Success;
            }
            try
            {
                MailAddress m = new MailAddress(value.ToString());

                return ValidationResult.Success;
            }
            catch (FormatException)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
