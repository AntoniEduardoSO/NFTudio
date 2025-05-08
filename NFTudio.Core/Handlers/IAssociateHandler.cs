using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Core.Handlers;
public interface IAssociateHandler
{
    public Task<Response<AssociateResponseDto?>> CreateAsync(CreateAssociateRequest request);
    public Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllAsync(GetAllAssociateRequest request);
    public Task<Response<Associate>> UpdateAsync(UpdateAssociateRequest request);
    public Task<Response<Associate>> GetByFilterAsync(GetByFilterAssociateRequest request);
}
