using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Core.Handlers;

public interface IAssociateHandler
{
    public Task<Response<AssociateResponseDto?>> CreateAsync(CreateAssociateRequest request);
    public Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllHomeAsync(GetAllAssociateHomeRequest request);
    public Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllManageAsync(GetAllAssociateManageRequest request);
    public Task<Response<AssociateResponseDto>> UpdateAsync(UpdateAssociateRequest request);
    public Task<Response<AssociateResponseDto>> GetByIdAsync(GetAssociateByIdRequest request);
    public Task<Response<AssociateResponseDto>> DeleteAsync(DeleteAssociateRequest request);
}
