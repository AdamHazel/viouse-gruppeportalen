using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Gruppeportalen.Areas.PrivateUser.Controllers;


[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class PersonsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly IPrivateUserOperations _puo;
    private readonly IUserPersonConnectionsService _upc;
    private readonly IPersonService _ps;
    private readonly IApplicationUserService _aus;
    private readonly IMembershipService _ms;

    public PersonsController(ApplicationDbContext db, UserManager<ApplicationUser> um,
        IPrivateUserOperations puo, IUserPersonConnectionsService upc, IPersonService ps, IApplicationUserService aus,
        IMembershipService ms)
    {
        _db = db;
        _um = um;
        _puo = puo;
        _upc = upc;
        _ps = ps;
        _aus = aus;
        _ms = ms;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var currentUser = _um.GetUserAsync(User).Result;
        var privateUser = _puo.GetPrivateUserByIdWithConnectedPersons(currentUser.Id);
        var specialView = new PrimaryPersonAndOthers
        {
            PrimaryPerson = new Person(),
            OtherPersons = new List<Person>(),
        };
        foreach (var connection in privateUser.UserPersonConnections)
        {
            if (connection.PersonId == connection.PrivateUserId)
            {
                specialView.PrimaryPerson = connection.Person;
                specialView.PrimaryPerson.PrimaryPerson = true;
            }
            else
            {
                connection.Person.PrimaryPerson = false;
                specialView.OtherPersons.Add(connection.Person);
            }
        }
        return View(specialView);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(new Person());
    }
    

    [HttpPost]
    public IActionResult Add(Person person)
    {
        if (ModelState.IsValid)
        {
            var privateUser = _um.GetUserAsync(User).Result;
            var resultOfAddingPerson = _ps.AddPersonToDbByPerson(person);

            if (resultOfAddingPerson.Result)
            {
                var resultOfAddingConnection = _upc.AddUserPersonConnection(privateUser.Id, person.Id);
                if (resultOfAddingConnection.Result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Oopsie",
                        new { Area = "", message = resultOfAddingPerson.Message});
                }
            }
            else
            {
                return RedirectToAction("ErrorMessage", "Oopsie",
                    new { Area = "", message = resultOfAddingPerson.Message});
            }
        } 
        return View(person); 
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var person = _puo.GetPersonDetails(id);
        if (person == null)
            return NotFound();
            
        return View("Edit", person);
    }

    [HttpPost]
    public IActionResult Edit(Person person)
    {
        if (!ModelState.IsValid) return View("Edit");
        _puo.EditPerson(person);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(string personId)
    {
        /*_puo.DeletePerson(id);*/
        var currentUser = _um.GetUserAsync(User).Result;
        
        var resultOfRemoval = _upc.DeleteUserPersonConnection(currentUser.Id, personId);
        if (resultOfRemoval.Result)
        {
            var otherConnections = _upc.DoesPersonHaveOtherConnections(personId);
            if (otherConnections)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_ps.RemovePersonFromDbById(personId))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Oopsie", 
                        new { Area = "", message = "Unable to remove person from DB" });
                }
            }
        }
        else
        {
            return RedirectToAction("ErrorMessage", "Oopsie", 
                new { Area = "", message = resultOfRemoval.Message });
        }
    }

    [HttpPost]
    public IActionResult SharePerson(string personId, string desiredEmail)
    {
        if (!_aus.IsUserPrivateUser(desiredEmail))
        {
            return Json(new { success = false, errorMessage = "Ugyldig e-postadresse. Brukeren må være registrert som private user." });
        }
        
        var user = _aus.GetPrivateUserByEmail(desiredEmail);

        if (_upc.DoesUserPersonConnectionExist(user.Id, personId))
        {
            return Json(new { success = false, errorMessage = "Personen er allerede delt med denne brukeren."});
        }

        if (_upc.IsPersonSharingLevelReached(personId))
        {
            return Json(new { success = false, errorMessage = "Denne personen har blitt delt to ganger. Det er ikke mulig å dele den mer." });
        }
        
        var resultOfAddingConnection = _upc.AddUserPersonConnection(user.Id, personId);
        if (!resultOfAddingConnection.Result)
        {
            return Json(new { success = false, errrorMessage = "Det oppsto en feil. Feil melding: " 
                                                               + resultOfAddingConnection.Message });
        }
        
        return Json(new {success = true});
    }



    [HttpPost]
    public IActionResult TransferPerson(string desiredEmail, string personId)
    {
        if (!_aus.IsUserPrivateUser(desiredEmail))
        {
            return Json(new { success = false, errorMessage = "Ugyldig e-postadresse. Brukeren må være registrert som private user." });
        }
        
        var desiredUser = _aus.GetPrivateUserByEmail(desiredEmail);
        var currentUser = _um.GetUserAsync(User).Result;

        if (desiredUser.Id == currentUser.Id)
        {
            return Json(new { success = false, errorMessage = "Du kan ikke overføre til deg selv." }); 
        }

        if (_upc.DoesUserPersonConnectionExist(desiredUser.Id, personId))
        {
            var resultOfRemoval = _upc.DeleteUserPersonConnection(currentUser.Id, personId);
            if (resultOfRemoval.Result)
            {
                return Json(new {success = true});
            }
            else
            {
                return Json(new { success = false, errorMessage = "Det oppsto en feil. Feil melding:" + " " + resultOfRemoval.Message });
            }
        }
        else
        {
            var resultOfAddingConnection = _upc.AddUserPersonConnection(desiredUser.Id, personId);
            if (resultOfAddingConnection.Result)
            {
                var resultOfRemoval = _upc.DeleteUserPersonConnection(currentUser.Id, personId);
                if (resultOfRemoval.Result)
                {
                    return Json(new {success = true});
                }
                else
                {
                    return Json(new { success = false, errorMessage = "Det oppsto en feil. Feil melding:" + " " + resultOfRemoval.Message });
                }
            }
            else
            {
                return Json(new { success = false, errorMessage = "Det oppsto en feil. Feil melding:" + " " + resultOfAddingConnection.Message }); 
            }
        }
    }

    [HttpPost]
    public IActionResult CancelMembership(Guid membershipId)
    {
        var membership = _ms.GetMembershipById(membershipId);
        if (membership == null)
        {
            return Json(new { success = false, message = "Fant ikke medlemsskapet" });
        }

        membership.ToBeRenewed = !membership.ToBeRenewed;
        var resultOfUpdating = _ms.UpdateMembership(membership);
        if (resultOfUpdating == null)
        {
            return Json(new { success = false, message = "Error" });
        }

        if (!resultOfUpdating.Result)
        {
            return Json(new { success = false, message = resultOfUpdating.Message });
        }
        
        return Json(new {success = true, message ="Success!" });
    }
}