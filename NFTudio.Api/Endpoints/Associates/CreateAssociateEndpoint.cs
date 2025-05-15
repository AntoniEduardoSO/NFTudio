using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Associates;
public class CreateAssociateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    =>
        app.MapPost("/", HandleAsync)
        .WithName("Associates: Create")
        .WithSummary("Cria uma nova empresa parceira")
        .WithDescription("Cria uma nova empresa parceira")
        .WithOrder(1)
        .Produces<Response<AssociateResponseDto>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAssociateHandler handler,
        CreateAssociateRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        var result = await handler.CreateAsync(request);

        return result.IsSuccess
            ? TypedResults.Created($"/{result}", result)
            : TypedResults.BadRequest(result.Data);
    }
}
