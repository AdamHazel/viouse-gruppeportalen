using System.Security.Principal;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _db;
    private readonly IPrivateUserOperations _pos;

    public PaymentService(ApplicationDbContext db, IPrivateUserOperations pos)
    {
        _db = db;
        _pos = pos;
    }

    private Payment? _fetchPaymentById(Guid paymentId)
    {
        return _db.Payments.FirstOrDefault(p => p.Id == paymentId);
    }

    private bool _addPayment(Payment p)
    {
        try
        {
            _db.Payments.Add(p);
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
    
    private bool _addMembershipPayment(Guid paymentId, Guid membershipId)
    {
        try
        {
            var payment = _fetchPaymentById(paymentId);
            var membership = _db.Memberships.FirstOrDefault(m => m.Id == membershipId);

            if (membership == null || payment == null)
            {
                throw new Exception("Unable to find membership or payment");
            }

            var mp = new MembershipPayment
            {
                PaymentId = payment.Id,
                MembershipId = membershipId,
            };
            
            payment.MembershipPayments.Add(mp);
            membership.MembershipPayments.Add(mp);
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

    private bool _doesPaymentExist(Guid paymentId)
    {
        return _db.Payments.Any(p => p.Id == paymentId);
    }

    private bool _removePayment(Payment p)
    {
        try
        {
            var user = p.PaidByUser;
            if (user != null)
            {
                user.Payments.Remove(p);
            }
            _db.Payments.Remove(p);
            if (_db.SaveChanges() > 0)
            {
                return true;
            }
            else throw new Exception("Failed to remove payment");
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
    
    public ResultOfOperation? AddPayment(Payment p)
    {
        var resultOfOperation = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty
        };

        if (_addPayment(p))
        {
            resultOfOperation.Result = true;
            return resultOfOperation;
        }
        else
        {
            resultOfOperation.Message = "Failed to add record of payment to db. Please try again.";
            return resultOfOperation;
        }
    }

    public ResultOfOperation? AddMemberPayment(Guid paymentId, Guid membershipId)
    {
        var resultOfOperation = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty
        };

        if (!_db.Memberships.Any(m => m.Id == membershipId))
        {
            resultOfOperation.Message = "Unable to find membership";
            return resultOfOperation;
        }

        if (!_doesPaymentExist(paymentId))
        {
            resultOfOperation.Message = "Payment not found";
            return resultOfOperation;
        }

        if (_addMembershipPayment(paymentId, membershipId))
        {
            resultOfOperation.Result = true;
            return resultOfOperation;
        }
        else
        {
            resultOfOperation.Message = "Failed to add MembershipPayment record";
            return resultOfOperation;
        }
    }

    public Payment? GetPaymentById(Guid id)
    {
        return _db.Payments
            .Include(p => p.MembershipPayments)
            .Include(p => p.PaidByUser)
            .FirstOrDefault(p => p.Id == id);
    }

    public ResultOfOperation? RemovePaymentById(Guid paymentId)
    {
        var roo = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty
        };
        
        var payment = GetPaymentById(paymentId);
        if (payment == null)
        {
            roo.Message = "Det var ikke mulig å finne betalingsinfo";
            return roo;
        }

        roo.Result = _removePayment(payment);
        if (!roo.Result)
        {
            roo.Message = "Det var ikke mulig å fjerne betalingsdata";
        }

        return roo;
    }
}