using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;
using NFTudio.Api.Models;

namespace NFTudio.Api.Common;

public static class AppExtension
{
    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapSwagger().RequireAuthorization();
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void SeedSql(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.Migrate();

        if (!db.Operations.Any())
        {
            var sql = File.ReadAllText("Data/seed.sql");

            db.ChangeTracker.AutoDetectChangesEnabled = false;
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            using var transaction = db.Database.BeginTransaction();
            db.Database.ExecuteSqlRaw(sql);
            transaction.Commit();
        }
    }

    public static async Task SeedUsersAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<long>(role));
            }
        }

        if (await userManager.FindByEmailAsync("admin@nftudio.com") == null)
        {
            var user = new User
            {
                UserName = "admin@nftudio.com",
                Email = "admin@nftudio.com",
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}