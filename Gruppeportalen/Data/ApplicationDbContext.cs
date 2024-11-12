using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    
    public DbSet<LocalGroupAdmin> LocalGroupAdmins { get; set; }

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
    }
}
