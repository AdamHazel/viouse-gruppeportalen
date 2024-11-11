using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.CentralOrganisation.Models;

[PrimaryKey(nameof(LocalGroupId), nameof(PrivateUserId))]
public class LocalGroupAdmin
{
    public Guid LocalGroupId { get; set; }
    public string PrivateUserId { get; set; } = string.Empty;
}