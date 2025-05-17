using System.Security.Claims;
using NFTudio.Api.Common;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Links;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Links;

public class UpdateLinkEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPut("{associateid}/link/{id}", HandleAsync)
        .WithName("Links: Update")
        .WithSummary("Atualiza um link de uma empresa")
        .WithDescription("Atualiza um link de uma empresa")
        .Produces<Response<LinkDto>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ILinkHandler handler,
        UpdateLinkRequest request,
        long associateid,
        long id)
    {

        request.Id = id;
        request.AssociateId = associateid;

        var result = await handler.UpdateAsync(request);

        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}