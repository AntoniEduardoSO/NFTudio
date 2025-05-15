using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NFTudio.Api.Common;
using NFTudio.Core;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Associates;
public class GetAllAssociateManageEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/manage", HandleAsync)
        .WithName("Associates: Get All Manage")
        .WithSummary("Recupera todas as empresas parceiras (Manage)")
        .WithDescription("Recupera todas as empresas parceiras (Manage)")
        .RequireAuthorization()
        .Produces<PagedResponse<ICollection<AssociateResponseDto>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAssociateHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] string search = "",
        [FromQuery] string? situation = "",
        [FromQuery] string[]? operationNames = null)
    {
        var request = new GetAllAssociateManageRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Search = search,
            Situation = situation,
            OperationNames = operationNames,
        };

        var result = await handler.GetAllManageAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}