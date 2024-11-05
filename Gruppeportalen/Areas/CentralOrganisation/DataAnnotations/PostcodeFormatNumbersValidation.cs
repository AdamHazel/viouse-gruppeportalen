using System.ComponentModel.DataAnnotations;

namespace Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;

public class PostcodeFormatNumbersValidation : ValidationAttribute
{
    private const string PostcodeValidationMessage = "Postnummer kan ikke inneholde bokstaver.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && value is string postcode)
        {
            foreach (var c in postcode)
            {
                if (!char.IsDigit(c))
                {
                    return new ValidationResult(PostcodeValidationMessage);
                }
            }
            return ValidationResult.Success;
        }
        
        return new ValidationResult(PostcodeValidationMessage);
    }
}