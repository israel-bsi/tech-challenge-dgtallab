using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Repositories;

public interface ICollaboratorRepository
{
    Task<Response<Collaborator>> AddAsync(Collaborator collaborator);
    Task<Response<Collaborator>> UpdateAsync(Collaborator request);
    Task<Response<Collaborator>> DeleteAsync(Collaborator collaborator);
    Task<Response<Collaborator>> GetByIdAsync(int id);
    Task<Response<Collaborator>> GetByCpfAsync(string cpf);
    Task<Response<Collaborator>> GetByRgAsync(string rg);
    Task<Response<IEnumerable<Department>>> GetAllDepartmentsAsync();
    Task<Response<IEnumerable<Collaborator>>> GetCollaboratorsByDepartmentIdsAsync(List<int> departmentIds, int excludeManagerId);
    Task<PagedResponse<IEnumerable<Collaborator>>> GetAllAsync(PagedRequest request);
}