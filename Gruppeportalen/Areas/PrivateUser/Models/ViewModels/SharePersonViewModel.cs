namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class SharePersonViewModel
{
    public SharePersonModel SharePersonModel { get; set; }
    public IEnumerable<Person> AllPersons { get; set; } = new List<Person>();
}