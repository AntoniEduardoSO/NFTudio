using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;

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
}