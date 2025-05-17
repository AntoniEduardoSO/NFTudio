using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Links;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Links;

public class CreateLinkEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/{associateid}/link/{id}", HandleAsync)
        .WithName("Links: Create")
        .WithSummary("Cria um ou mais links para a empresa")
        .WithDescription("Cria um ou mais links para a empresa")
        .Produces<Response<ICollection<LinkDto>>>();


    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ILinkHandler handler,
        CreateLinkRequest request)
    {
        var result = await handler.CreateAsync(request);

        return result.IsSuccess
            ? TypedResults.Created($"/{result}", result)
            : TypedResults.BadRequest(result.Data);
    }
}