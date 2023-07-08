using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validators
{
    public class OnlyNumbers : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return (value != null) && ((string)value).All(char.IsNumber);
        }
    }
}
