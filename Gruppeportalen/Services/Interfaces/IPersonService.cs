using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IPersonService
{
    string GeneratePersonId();
    Person? CreatePrimaryPersonByUser(PrivateUser user);
    
    bool AddPersonToDbByPerson(Person person);
    Person? GetPersonById(string personId);
}