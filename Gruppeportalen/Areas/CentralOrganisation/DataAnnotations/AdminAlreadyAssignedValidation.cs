using System.ComponentModel.DataAnnotations;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;

namespace Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;

public class AdminAlreadyAssignedValidation : ValidationAttribute
{
    private const string ErrorValidationMessage = "Admin med denne epostadressen eksisterer allerede.";
    private const string NullMessage = "Error... unable validate local group id"; 
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        
        var db = (ApplicationDbContext)validationContext.GetRequiredService(typeof(ApplicationDbContext));
        
        if (db == null)
            throw new InvalidOperationException("Db context is null.");
        
        var model = validationContext.ObjectInstance;
        var localGroupProperty = model.GetType().GetProperty("LocalGroupId");
        var localGroupId = localGroupProperty?.GetValue(model) as Guid?;
        if (localGroupId != null)
        {
            var email = value.ToString().ToLower();
            var pu = db.Users.FirstOrDefault(u => u.Email == email);
            if (pu != null)
            {
                var lga = db.LocalGroupAdmins.FirstOrDefault(lga => lga.LocalGroupId == localGroupId && lga.UserId == pu.Id);
        
                if (pu != null && lga == null)
                    return ValidationResult.Success;
                else
                {
                    return new ValidationResult(ErrorValidationMessage);  
                }
            }
            else
            {
                return new ValidationResult(NullMessage);
            }
        }
        else
        {
            return new ValidationResult(NullMessage);
        }
    }
}