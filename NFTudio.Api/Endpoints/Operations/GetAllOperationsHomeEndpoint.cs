using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NFTudio.Api.Common;
using NFTudio.Core;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Operations;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Endpoints.Operations;
public class GetAllOperationsHomeEnpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/home", HandleAsync)
        .WithName("Operation: Get All Home")
        .WithSummary("Recupera todas as atuacoes parceiras (Home)")
        .WithDescription("Recupera todas as atuacoes parceiras (Home)")
        .Produces<PagedResponse<ICollection<Operation>?>>();

    private static async Task<IResult> HandleAsync(
        IOperationHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize
        )
    {
        var request = new GetAllOperationsHomeRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await handler.GetAllHomeAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}