
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NormalUser = Gruppeportalen_new.Models.NormalUser;

namespace Gruppeportalen_new.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<NormalUser> NormalUsers { get; set; }
    public DbSet<OrganisationUser> OrganisationUsers { get; set; }
}
