namespace Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;

public interface ICentralOrganisationService
{ 
   Models.CentralOrganisation _getCentralOrganisations(string userId);

   void _editOrganisationDetails(Models.CentralOrganisation viewModel);

}