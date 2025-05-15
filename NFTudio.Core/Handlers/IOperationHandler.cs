using NFTudio.Core.Models;
using NFTudio.Core.Requests.Operations;
using NFTudio.Core.Responses;

namespace NFTudio.Core.Handlers;
public interface IOperationHandler
{
    public Task<PagedResponse<ICollection<Operation>>> GetAllHomeAsync(GetAllOperationsHomeRequest request);
}
