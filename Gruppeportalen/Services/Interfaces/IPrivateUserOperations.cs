using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IPrivateUserOperations
{
    bool AddPrivateUserToDb(PrivateUser privateUser);
    ApplicationPrivateUser? GetUserDetails(string userId);
    Person? GetPersonDetails(string personId);
    void EditPerson(Person person);
    void EditUserDetails(ApplicationPrivateUser viewModel);
    IEnumerable<LocalGroup> GetAllLocalActiveGroups();
    IEnumerable<LocalGroup> SearchLocalGroups(string query, string county);
    bool PrivateUserExists(string id);
    PrivateUser? GetPrivateUserById(string id);
   
    PrivateUser? GetPrivateUserByIdWithConnectedPersons(string id);
}