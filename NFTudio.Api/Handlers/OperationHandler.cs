using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Operations;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Handlers;
public class OperationHandler(AppDbContext context) : IOperationHandler
{
    public async Task<PagedResponse<ICollection<Operation>>> GetAllHomeAsync(GetAllOperationsHomeRequest request)
    {

        var query = context.AssociateOperations
            .AsNoTracking()
            .Where(ao => ao.Associate.Situation == "Parceria Concluída e Publicada")
            .GroupBy(ao => new { ao.Operation.Id, ao.Operation.Name })
            .OrderByDescending(g => g.Count())
            .Select(g => new Operation
            {
                Id = g.Key.Id,
                Name = g.Key.Name
            });
    



        var operations = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        
        var count = await query.CountAsync();

        return new PagedResponse<ICollection<Operation>>(operations, count, request.PageNumber, request.PageSize);
    }
}