using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class MembershipService : IMembershipService
{
    private readonly ApplicationDbContext _db;
    private readonly IPersonService _ps;
    private readonly ILocalGroupService _lgs;
    private readonly IMembershipTypeService _mts;
    private readonly IPaymentService _pay;

    public MembershipService(ApplicationDbContext db, IPersonService ps, ILocalGroupService lgs, 
        IPaymentService pay, IMembershipTypeService mts)
    {
        _db = db;
        _ps = ps;
        _lgs = lgs;
        _pay = pay;
        _mts = mts;
    }

    private Membership? _getMembershipById(Guid id)
    {
        return _db.Memberships
            .Include(m => m.MembershipPayments)
            .Include(m => m.MembershipType)
            .Include(m => m.LocalGroup)
            .Include(m => m.Person)
            .FirstOrDefault(m => m.Id == id);
    }

    private Membership? _getMembershipByGroupMembershipTypeAndPersonId
    (Guid membershipTypeId, string personId, Guid localGroupId)
    {
        return _db.Memberships
            .Include(m => m.MembershipPayments)
            .Include(m => m.MembershipType)
            .FirstOrDefault(m => membershipTypeId == m.MembershipTypeId
            && m.PersonId == personId && m.LocalGroupId == localGroupId);
    }
    private Membership? _addMembership(MembershipType mt, Person p, LocalGroup lg)
    {
        try
        {
            var currentMonth = DateTime.Now.Month;
            var currentDay = DateTime.Now.Day;
            var currentYear = DateTime.Now.Year;

            var yearForReset = currentYear;

            if (mt.MonthReset < currentMonth)
            {
                yearForReset = currentYear + 1;
            } 
            else if (mt.MonthReset == currentMonth)
            {
                if (mt.DayReset <= currentDay)
                {
                    yearForReset = currentYear + 1;
                }
            }

            var newMembership = new Membership
            {
                StartDate = DateTime.Now,
                EndDate = new DateTime(yearForReset, mt.MonthReset, mt.DayReset, 23, 59, 59, DateTimeKind.Utc),
                IsActive = false,
                IsBlocked = false,
                ToBeRenewed = true,
            };
            
            newMembership.MembershipTypeId = mt.Id;
            newMembership.LocalGroupId = lg.Id;
            newMembership.PersonId = p.Id;
            
            _db.Memberships.Add(newMembership);

            if (_db.SaveChanges() > 0)
            {
                return newMembership;
            }
            else throw new Exception("Failed to add membership");
        }
        catch (DbUpdateException exception)
        {
            return null;
        }
        catch (Exception exception)
        {
            return null;
        }
    }

    private bool _removeMembership(Membership m, MembershipType mt, LocalGroup lg, Person p)
    {
        try
        {
            mt.Memberships.Remove(m);
            lg.Memberships.Remove(m);
            p.Memberships.Remove(m);
            _db.Memberships.Remove(m);
            if (_db.SaveChanges() > 0)
            {
                return true;
            }
            else throw new Exception("Failed to remove membership");
        }
        catch (DbUpdateException exception)
        {
            return false;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    private bool _addMembershipPayment(Membership m, Payment p)
    {
        try
        {
            var mp = new MembershipPayment
            {
                MembershipId = m.Id,
                PaymentId = p.Id,
            };
            m.MembershipPayments.Add(mp);
            p.MembershipPayments.Add(mp);
            
            return (_db.SaveChanges() > 0);
        }
        catch (DbUpdateException exception)
        {
            return false;
        }
        catch (Exception exception)
        {
            return false;
        }
    }
    
    public ResultAndMemebership? AddMembershipToDatabase(Guid membershipTypeId, string personId, Guid localGroupId)
    {
        var result = new ResultAndMemebership
        {
            R = new ResultOfOperation
            {
                Result = false,
                Message = string.Empty,
            },
        };
        
        var group = _lgs.GetLocalGroupById(localGroupId);
        if (group == null)
        {
            result.R.Message = "Could not find local group needed to add membership";
            return result;
        }
        
        var person = _ps.GetPersonById(personId);
        if (person == null)
        {
            result.R.Message = "Could not find person needed to add membership";
            return result;
        }
        
        var membershipType = _mts.GetMembershipTypeById(membershipTypeId);
        if (membershipType == null)
        {
            result.R.Message = "Could not find membership type needed to add membership";
            return result;
        }

        var membership = _addMembership(membershipType, person, group);
        if (membership != null)
        {
            result.R.Result = true;
            result.M = _getMembershipById(membership.Id);
            return result;
        }
        else
        {
            result.R.Message = "Could not add membership";
            return result;
        }
    }

    public ResultOfOperation AddMembershipPaymentToDatabase(Guid membershipId, Guid paymentId)
    {
        var resultOfOperation = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty,
        };
        
        var membership = _getMembershipById(membershipId);
        if (membership == null)
        {
            resultOfOperation.Message = "Could not find membership needed to add MembershipPayment";
            return resultOfOperation;
        }
        
        var payment = _pay.GetPaymentById(paymentId);
        if (payment == null)
        {
            resultOfOperation.Message = "Could not find payment needed to add MembershipPayment";
            return resultOfOperation;
        }

        if (_addMembershipPayment(membership, payment))
        {
            resultOfOperation.Result = true;
            return resultOfOperation;
        }
        else
        {
            resultOfOperation.Message = "Could not add MembershipPayment";
            return resultOfOperation;
        }
            
    }
    public ResultOfOperation? AllowedToAddMembership(Guid membershipTypeId, string personId, Guid localGroupId)
    {
        var resultOfOperation = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty,
        };
        
        var membership = _getMembershipByGroupMembershipTypeAndPersonId(membershipTypeId, personId, localGroupId);

        if (membership == null)
        {
            resultOfOperation.Result = true;
            return resultOfOperation;
        }
        
        if (!membership.ToBeRenewed && !membership.IsActive)
        {
            resultOfOperation.Result = true;
            return resultOfOperation;
        }

        resultOfOperation.Message = "Medlemsskap for denne personen eksisterer allerede";
        return resultOfOperation;
    }

    public ResultOfOperation? RemoveMembershipById(Guid membershipId)
    {
        var roo = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty,
        };
        
        var membership = _getMembershipById(membershipId);
        if (membership == null)
        {
            roo.Message = "Kunne ikke finne medlemsskapet i systemet.";
            return roo;
        }
        
        var lg = membership.LocalGroup;
        var mt = membership.MembershipType;
        var p = membership.Person;

        roo.Result = _removeMembership(membership, mt, lg, p);
        if (!roo.Result)
        {
            roo.Message = "Ikke mulig å slette medlemsskap";
        }
        
        return roo;
    }
}