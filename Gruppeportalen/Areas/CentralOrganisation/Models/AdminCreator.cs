using System.ComponentModel.DataAnnotations;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Models;

public class AdminCreator
{
    [Required]
    [PrivateUserExistsValidation]
    public string AdminEmail { get; set; } = string.Empty;
    
    public Guid LocalGroupId { get; set; }
}