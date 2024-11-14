using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Models;

[PrimaryKey(nameof(LocalGroupId), nameof(UserId))]
public class LocalGroupAdmin
{
    public Guid LocalGroupId { get; set; }
    
    [ForeignKey(nameof(LocalGroupId))]
    public LocalGroup LocalGroup { get; set; } = null!;
    
    public string UserId { get; set; } = string.Empty;
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;
}