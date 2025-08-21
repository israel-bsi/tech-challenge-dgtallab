using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Repositories;

public interface ICollaboratorRepository
{
    Task<Response<Collaborator>> AddAsync(Collaborator collaborator);
    Task<Response<Collaborator>> UpdateAsync(Collaborator collaborator);
    Task<Response<Collaborator>> DeleteAsync(Collaborator collaborator);
    Task<Response<Collaborator>> GetByIdAsync(int id);
    Task<PagedResponse<IEnumerable<Collaborator>>> GetAllAsync(PagedRequest request);
}