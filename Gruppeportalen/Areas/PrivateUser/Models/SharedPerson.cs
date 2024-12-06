namespace Gruppeportalen.Areas.PrivateUser.Models;

public class SharedPerson
{
    public string PrivateUserId { get; set; }
    public Gruppeportalen.Models.PrivateUser PrivateUser { get; set; } 
    
    public string PersonId { get; set; }
    public Person Person { get; set; }
}