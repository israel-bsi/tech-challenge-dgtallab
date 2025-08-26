using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Handler;

public interface ICollaboratorHandler
{
    Task<Response<CollaboratorResponse>> AddAsync(EditCollaboratorRequest request);
    Task<Response<CollaboratorResponse>> UpdateAsync(EditCollaboratorRequest request);
    Task<Response<CollaboratorResponse>> DeleteAsync(int id);
    Task<Response<CollaboratorResponse>> GetByIdAsync(int id);
    Task<Response<IEnumerable<CollaboratorResponse>>> GetSubordinatesAsync(int managerId);
    Task<Response<IEnumerable<CollaboratorResponse>>> GetCollaboratorsByDepartment(int departmentId);
    Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request);
}