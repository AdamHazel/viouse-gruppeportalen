namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class CompleteLocalGroupOverview
{
    public List<AdminLocalGroupOverview>? AdminOverview { get; set; }
    public List<PersonLocalGroupOverview>? PersonOverview {get; set;}
}