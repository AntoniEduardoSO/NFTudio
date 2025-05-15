using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Models;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User,
        IdentityRole<long>,
        long,
        IdentityUserClaim<long>,
        IdentityUserRole<long>,
        IdentityUserLogin<long>,
        IdentityRoleClaim<long>,
        IdentityUserToken<long>>(options)
{
    public DbSet<Associate> Associates { get; set; } = null!;
    public DbSet<Link> Links { get; set; } = null!;
    public DbSet<AssociateOperation> AssociateOperations { get; set; } = null!;
    public DbSet<Operation> Operations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}