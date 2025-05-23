using NFTudio.Api.Data;
using NFTudio.Api.Handlers;
using NFTudio.Api.Models;
using NFTudio.Core;
using NFTudio.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NFTudio.Api.Common;

public static class BuilderExtension
{
    public static void AddConfiguration(
        this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString =
            builder
                .Configuration
                .GetConnectionString("DefaultConnection")
            ?? string.Empty;

        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        builder.Services.ConfigureApplicationCookie(opt =>
        {
            opt.Cookie.SameSite = SameSiteMode.None;     
            opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; 

            opt.Events.OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddAuthorization();
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddDbContext<AppDbContext>(
                x => { x.UseSqlite(Configuration.ConnectionString); });
        builder.Services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins(
                        Configuration.BackendUrl,
                        Configuration.FrontendUrl
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IAssociateHandler, AssociateHandler>();
        builder.Services.AddTransient<IOperationHandler, OperationHandler>();
        builder.Services.AddTransient<ILinkHandler, LinkHandler>();
    }


}