using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Associate> Associates { get; set; } = null!;
    public DbSet<Link> Links { get;set;} = null!;
    public DbSet<AssociateOperation> AssociateOperations { get; set; } = null!;
    public DbSet<Operation> Operations{ get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}