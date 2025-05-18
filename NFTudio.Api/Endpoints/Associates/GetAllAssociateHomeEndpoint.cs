using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NFTudio.Api.Common;
using NFTudio.Core;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Associates;
public class GetAllAssociateHomeEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/home", HandleAsync)
        .WithName("Associates: Get All Home")
        .WithSummary("Recupera todas as empresas parceiras (Home)")
        .WithDescription("Recupera todas as empresas parceiras (Home)")
        .Produces<PagedResponse<ICollection<AssociateResponseDto>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IAssociateHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] string search = "",
        [FromQuery] string? location = "",
        [FromQuery] string[]? operationNames = null)
    {
        var request = new GetAllAssociateHomeRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Search = search,
            Location = location,
            OperationNames = operationNames
        };

        var result = await handler.GetAllHomeAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}