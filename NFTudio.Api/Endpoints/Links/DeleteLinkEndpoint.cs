using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Links;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Links;

public class DeleteLinkEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("{associateid}/link/{id}", HandleAsync)
        .WithName("Links: Delete")
        .WithSummary("Deleta um link de uma empresa")
        .WithDescription("Deleta um link de uma empresa")
        .Produces<Response<LinkDto>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ILinkHandler handler,
        long associateid,
        long id)
    {

        var request = new DeleteLinkRequest
        {
            Id = id,
            AssociateId = associateid
        };

        var result = await handler.DeleteAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
