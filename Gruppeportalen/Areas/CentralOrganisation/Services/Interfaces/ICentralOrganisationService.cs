namespace Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;

public interface ICentralOrganisationService
{ 
   Models.CentralOrganisation? GetCentralOrganisationByUser(string userId);

   void EditOrganisationDetails(Models.CentralOrganisation viewModel);

}