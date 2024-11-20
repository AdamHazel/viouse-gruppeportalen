using Gruppeportalen.Areas.PrivateUser.DataAnnotations;
namespace Gruppeportalen.Areas.PrivateUser.Models;

public class SharedPerson
{
    [PrivateUserExistsValidation2]
    public string PrivateUserId { get; set; }
    public PrivateUser PrivateUser { get; set; } 
    
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}