using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Gruppeportalen.Data;
using Microsoft.VisualBasic;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models;
using Constants = Gruppeportalen.HelperClasses.Constants;


namespace Gruppeportalen.Areas.PrivateUser.DataAnnotations;

public class PrivateUserExistsValidation2 : ValidationAttribute
{
    private const string PrivateUserExistsValidationMessage =
        "Private user med denne eposten eksisterer ikke i systemet.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var db = (ApplicationDbContext)validationContext.GetRequiredService(typeof(ApplicationDbContext));
        
        if (db == null)
            throw new InvalidOperationException("Db context is null");

        var email = value.ToString().ToLower();
        var pu = db.Users.FirstOrDefault(u => u.Email == email);
        if (pu != null && pu.TypeOfUser == Constants.Privateuser)
            return ValidationResult.Success;

        return new ValidationResult(PrivateUserExistsValidationMessage);
    }
}