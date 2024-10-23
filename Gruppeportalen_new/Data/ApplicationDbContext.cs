using Gruppeportalen_new.Areas.NormalUser.Models;
using Gruppeportalen_new.Areas.OrganisationUser.Models;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
