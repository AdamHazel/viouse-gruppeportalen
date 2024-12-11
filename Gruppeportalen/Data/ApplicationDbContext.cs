using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Connection = NuGet.Protocol.Plugins.Connection;

namespace Gruppeportalen.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<PrivateUser> PrivateUsers { get; set; }
    public DbSet<CentralOrganisation> CentralOrganisations { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<LocalGroup> LocalGroups { get; set; }
    
    public DbSet<UserPersonConnection> UserPersonConnections { get; set; }
    public DbSet<LocalGroupAdmin> LocalGroupAdmins { get; set; }
    
    public DbSet<MembershipType> MembershipTypes { get; set; }
    
    public DbSet<Membership> Memberships { get; set; }
    
    public DbSet<Payment> Payments { get; set; }
    
    public DbSet<MembershipPayment> MembershipPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<LocalGroupAdmin>()
            .HasKey(lga => new { lga.LocalGroupId, lga.UserId });
        
        builder.Entity<LocalGroupAdmin>()
            .HasOne(lga => lga.LocalGroup)
            .WithMany(lg => lg.LocalGroupAdmins)
            .HasForeignKey(lga => lga.LocalGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<LocalGroupAdmin>()
            .HasOne(lga => lga.User)
            .WithMany(pu => pu.LocalGroupAdmins)
            .HasForeignKey(lga => lga.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<LocalGroup>()
            .HasOne(co => co.Organisation)
            .WithMany(co => co.LocalGroups)
            .HasForeignKey(co => co.CentralOrganisationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Person>()
            .HasOne(p => p.PrivateUser);

        builder.Entity<UserPersonConnection>()
            .HasKey(upc => new { upc.PersonId, upc.PrivateUserId });
        
        builder.Entity<UserPersonConnection>()
            .HasOne(upc => upc.Person)
            .WithMany(per => per.UserPersonConnections)
            .HasForeignKey(upc => upc.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserPersonConnection>()
            .HasOne(upc => upc.PrivateUser)
            .WithMany(pu => pu.UserPersonConnections)
            .HasForeignKey(upc => upc.PrivateUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<MembershipType>()
            .HasKey(mt => new { mt.Id });
        
        builder.Entity<MembershipType>()
            .HasOne(mt => mt.LocalGroup)
            .WithMany(g => g.MembershipTypes)
            .HasForeignKey(mt => mt.LocalGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Membership>()
            .HasOne(m => m.LocalGroup)
            .WithMany(g => g.Memberships)
            .HasForeignKey(mt => mt.LocalGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Membership>()
            .HasOne(m => m.Person)
            .WithMany(p => p.Memberships)
            .HasForeignKey(mt => mt.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Membership>()
            .HasOne(m => m.MembershipType)
            .WithMany(mt => mt.Memberships)
            .HasForeignKey(mt => mt.MembershipTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<MembershipPayment>()
            .HasKey(mt => new { mt.MembershipId, mt.PaymentId });

        builder.Entity<MembershipPayment>()
            .HasOne(mp => mp.Membership)
            .WithMany(mp => mp.MembershipPayments)
            .HasForeignKey(mp => mp.MembershipId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<MembershipPayment>()
            .HasOne(mp => mp.Payment)
            .WithMany(mp => mp.MembershipPayments)
            .HasForeignKey(mp => mp.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
