using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Associates;

public class DeleteAssociateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("/{id}", HandleAsync)
        .WithName("Associate: Delete")
        .WithSummary("Exclui uma empresa parceira")
        .WithDescription("Exclui uma empresa parceira")
        .Produces<Response<AssociateResponseDto>>()
        .RequireAuthorization();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAssociateHandler handler,
        long id)
    {
        var request = new DeleteAssociateRequest
        {
            Id = id
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}