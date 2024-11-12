using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.CentralOrganisation.Models.ViewModels;

public class EditLocalGroupViewModel
{
    public LocalGroup? LocalGroup { get; set; }
    public AdminCreator? AdminCreator { get; set; }
    public List<ApplicationUser>? LocalGroupAdmins { get; set; }
}