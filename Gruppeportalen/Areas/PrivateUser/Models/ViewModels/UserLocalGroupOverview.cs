namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class UserLocalGroupOverview
{
    public Guid LocalGroupId { get; set; }
    public string LocalGroupName {get; set;} = string.Empty;
    public string UserStatus {get; set;} = String.Empty;
}