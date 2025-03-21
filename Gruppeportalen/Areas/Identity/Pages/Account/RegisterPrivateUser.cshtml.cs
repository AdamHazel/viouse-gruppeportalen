﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using PrivateUserModel = Gruppeportalen.Models.PrivateUser;

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class RegisterPrivateUserModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RegisterPrivateUserModel> _logger;
        private readonly IPrivateUserOperations _puo;
        private readonly IPersonService _ps;
        private readonly IUserPersonConnectionsService _upcs;

        public RegisterPrivateUserModel(ApplicationDbContext db, ILogger<RegisterPrivateUserModel> logger,
            IPrivateUserOperations puo, IPersonService ps, IUserPersonConnectionsService upcs)
        {
            _db = db;
            _logger = logger;
            _puo = puo;
            _ps = ps;
            _upcs = upcs;
        }

        [BindProperty]
        public PrivateUserInputModel Input { get; set; } = new();

        public class PrivateUserInputModel
        {
            public string UserId { get; set; } // ID from ApplicationUser

            // Private user fields
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Telephone { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            
            [MinLength(4)]
            [StringLength(4)]
            [PostcodeFormatNumbersValidation]
            public string Postcode { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return new JsonResult(new { success = false, errors });
            }

            // Add the new private user record to the database
            var privateUser = new PrivateUserModel
            {
                Id = Input.UserId,
                Firstname = Input.Firstname,
                Lastname = Input.Lastname,
                DateOfBirth = Input.DateOfBirth,
                Address = Input.Address,
                City = Input.City,
                Postcode = Input.Postcode,
                Telephone = Input.Telephone
            };

            _db.PrivateUsers.Add(privateUser);
            await _db.SaveChangesAsync();
            
            var person = _ps.CreatePrimaryPersonByUser(privateUser);
            if (person != null)
            {
                var resultOfAddingPerson= _ps.AddPersonToDbByPerson(person);
                var resultOfAddingConnection = _upcs.AddUserPersonConnection(privateUser.Id, person.Id);
            }
            
            return new JsonResult(new { success = true });
        }
    }
}
