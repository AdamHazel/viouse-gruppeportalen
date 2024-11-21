namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class CompleteLocalGroupOverview
{
    public List<UserLocalGroupOverview>? UserOverview { get; set; }
    public List<PersonLocalGroupOverview>? PersonOverview {get; set;}
}