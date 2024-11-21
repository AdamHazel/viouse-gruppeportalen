using Gruppeportalen.DataAnnotations;
namespace Gruppeportalen.Areas.PrivateUser.Models;

public class SharePersonModel
{ 
    [PrivateUserExistsValidation] 
    public string Email { get; set; } = string.Empty;
    
    public Guid PersonId { get; set; }
}