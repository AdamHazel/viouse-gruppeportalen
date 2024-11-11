namespace Gruppeportalen.Areas.PrivateUser.Models;

public class SharedPerson
{
    public string PrivateUserId { get; set; }
    public PrivateUser PrivateUser { get; set; } 
    
    public Guid PersonId { get; set; }
    public Person Person { get; set; }
}