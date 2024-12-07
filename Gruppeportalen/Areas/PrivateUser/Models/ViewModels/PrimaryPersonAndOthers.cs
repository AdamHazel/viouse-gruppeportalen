namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class PrimaryPersonAndOthers
{
    public Person? PrimaryPerson { get; set; }
    public List<Person>? OtherPersons { get; set; }
}