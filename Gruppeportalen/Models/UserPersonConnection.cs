using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.PrivateUser.Models;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Models;

[PrimaryKey(nameof(PrivateUserId), (nameof(PersonId)))]
public class UserPersonConnection
{
    public string PrivateUserId { get; set; } = string.Empty;
    [ForeignKey(nameof(PrivateUserId))]
    public PrivateUser PrivateUser { get; set; } = null!;
    
    public string PersonId { get; set; } = string.Empty;
    [ForeignKey(nameof(PersonId))]
    public Person Person { get; set; } = null!;
}