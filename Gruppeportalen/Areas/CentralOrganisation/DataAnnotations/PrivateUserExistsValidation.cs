using System.ComponentModel.DataAnnotations;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;

public class PrivateUserExistsValidation : ValidationAttribute
{
    private const string PrivateUserExistsValidationMessage = "Private user med denne eposten eksister ikke på systemet.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        
        var db = (ApplicationDbContext)validationContext.GetRequiredService(typeof(ApplicationDbContext));
        
        if (db == null)
            throw new InvalidOperationException("Db context is null.");
        
        var email = value as string;
        var pu = db.Users.FirstOrDefault(u => u.Email == email);
        if (pu != null && pu.TypeOfUser == Constants.Privateuser)
            return ValidationResult.Success;
        
        return new ValidationResult(PrivateUserExistsValidationMessage);
    }
}