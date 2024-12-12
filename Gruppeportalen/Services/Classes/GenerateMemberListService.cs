using System.Text;
using Gruppeportalen.Data;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class GenerateMemberListService:IGenerateMemberList
{ 
    private readonly ApplicationDbContext _db;
    
    public GenerateMemberListService(ApplicationDbContext db)
    {
        _db = db;
    }

    
    public bool IsMembershipListEmpty(Guid localgroupid)
    {
        return !_db.Memberships.Any(m => m.IsActive && m.LocalGroupId == localgroupid);
    }
    
    public byte[] GenerateActiveMembershipsCsv(Guid localgroupid)
    {
        Console.WriteLine($"Filtering memberships for LocalGroupId: {localgroupid}");

        var activeMemberships = _db.Memberships
            .Include(m => m.Person)
            .Include(m => m.MembershipType)
            .Where(m => m.IsActive)
            .Where(m => m.LocalGroupId == localgroupid)
            .ToList();

        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("ID;Fornavn;Etternavn;Adresse;Fødselsdato;Medlemskapstype;Startdato;Status Aktiv;Status Blokkert");

        if (activeMemberships.Any())
        {
            foreach (var membership in activeMemberships)
            {
                csvBuilder.AppendLine(
                    $"{membership.Id};" +
                    $"{membership.Person.Firstname};" +
                    $"{membership.Person.Lastname};" +
                    $"\"{membership.Person.Address}, {membership.Person.Postcode}, {membership.Person.City}\";" +
                    $"{membership.Person.DateOfBirth:dd.MM.yyyy};" +
                    $"{membership.MembershipType.MembershipName};" +
                    $"{membership.StartDate:dd.MM.yyyy};" +
                    $"{(membership.IsActive ? "Ja" : "Nei")};" +
                    $"{(membership.IsBlocked ? "Ja" : "Nei")}"
                );
            }
        }
        else
        {
            Console.WriteLine("Ingen aktive medlemskap funnet. Genererer tom CSV med kun header.");
        }

     
        var utf8Bom = Encoding.UTF8.GetPreamble();
        var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return utf8Bom.Concat(csvBytes).ToArray();
    }



}
