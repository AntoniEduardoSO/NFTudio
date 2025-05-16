using NFTudio.Api.Common;
using NFTudio.Api.Endpoints.Associates;
using NFTudio.Api.Endpoints.Identity;
using NFTudio.Api.Endpoints.Operations;
using NFTudio.Api.Models;
using NFTudio.Core.Requests.Associate;

namespace NFTudio.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("");

        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup("v1/associate")
            .WithTags("Associates")
            .MapEndpoint<CreateAssociateEndpoint>()
            .MapEndpoint<GetAllAssociateManageEndpoint>()
            .MapEndpoint<UpdateAssociateEndpoint>()
            .MapEndpoint<GetAllAssociateHomeEndpoint>()
            .MapEndpoint<DeleteAssociateEndpoint>();

        endpoints.MapGroup("v1/operation")
            .WithTags("Operations")
            .MapEndpoint<GetAllOperationsHomeEnpoint>();

        endpoints.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapIdentityApi<User>();

        endpoints.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapEndpoint<LogoutEndpoint>();

    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
