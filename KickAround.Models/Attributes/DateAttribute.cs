using System;
using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime) value;

            if (date < DateTime.Today || date > DateTime.Today.AddYears(1))
            {
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The date must be in the range {DateTime.Today} and {DateTime.Today.AddYears(1)}.";
        }
    }
}
