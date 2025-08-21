using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Repositories;

public interface ICollaboratorRepository
{
    Task<Response<CollaboratorResponse>> AddAsync(EditCollaboratorRequest request);
    Task<Response<CollaboratorResponse>> UpdateAsync(EditCollaboratorRequest request);
    Task<Response<CollaboratorResponse>> DeleteAsync(int id);
    Task<Response<CollaboratorResponse>> GetByIdAsync(int id);
    Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request);
}