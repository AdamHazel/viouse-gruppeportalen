using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IPersonService
{
    string GeneratePersonId();
    Person? CreatePrimaryPersonByUser(PrivateUser user);
    
    ResultOfOperation AddPersonToDbByPerson(Person person);
    Person? GetPersonById(string personId);
    bool RemovePersonFromDbById(string personId);
    
}