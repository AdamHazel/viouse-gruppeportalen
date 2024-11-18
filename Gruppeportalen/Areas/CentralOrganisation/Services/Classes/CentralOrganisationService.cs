using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Classes;

public class CentralOrganisationService:ICentralOrganisationService
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;

    public CentralOrganisationService(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }
    
    public Models.CentralOrganisation _getCentralOrganisations(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return null;
        }
        var organisation = _db.CentralOrganisations
            .Include(co => co.ApplicationUser) 
            .FirstOrDefault(co => co.Id == userId);

        return organisation;
    }

    public void _editOrganisationDetails(Models.CentralOrganisation viewModel)
    {
       
        try
        {
            var organisation = _db.CentralOrganisations.FirstOrDefault(co => co.Id == viewModel.Id);
            
            if (organisation == null)
            {
                throw new NullReferenceException();
            }
            organisation.OrganisationName = viewModel.OrganisationName;
            organisation.OrganisationNumber = viewModel.OrganisationNumber;
            _db.SaveChanges();

        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("En feil oppstod: {ex.Message}");
        }
    }
}