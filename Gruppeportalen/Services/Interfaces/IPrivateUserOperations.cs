﻿using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IPrivateUserOperations
{
    bool AddPrivateUserToDb(PrivateUser privateUser);
    bool CreatePersonConnectedToPrivateUser(PrivateUser privateUser);
    bool AddPersonToPrivateUser(string privateUserId, Person person);
    bool AddNewPerson(string privateUserId, Person person);
    ApplicationPrivateUser? GetUserDetails(string userId);
    Person GetPersonDetails(Guid personId);
    void EditPerson(Person person);
    void EditUserDetails(ApplicationPrivateUser viewModel);
    void DeletePerson(Guid personId);
    IEnumerable<LocalGroup> GetAllLocalActiveGroups();
    IEnumerable<LocalGroup> SearchLocalGroups(string query, string county);
    bool PrivateUserExists(string id);
    PrivateUser? GetPrivateUser(string id);
    public List<Person> GetAllPersons(string privateUserId);
    public void TransferPerson(string newOwnerEmail, Guid personId);
    public void SharePersonWithUser(string email, Guid personId);
}