using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Associates;
public class UpdateAssociateEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPut("/{id}", HandleAsync)
        .WithName("Associates: Update")
        .WithSummary("Atualiza a empresa parceira")
        .WithDescription("Atualiza a empresa parceira")
        .RequireAuthorization()
        .Produces<Response<AssociateResponseDto>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAssociateHandler handler,
        UpdateAssociateRequest request,
        long id)
    {
        request.Id = id;
        request.UserId = user.Identity?.Name ?? string.Empty;

        var result = await handler.UpdateAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}