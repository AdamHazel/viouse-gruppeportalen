using System.ComponentModel.DataAnnotations;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Gruppeportalen.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Models;

public class AdminCreator
{
    [Required]
    [PrivateUserExistsValidation]
    [AdminAlreadyAssignedValidation]
    public string AdminEmail { get; set; } = string.Empty;
    
    public Guid LocalGroupId { get; set; }
}