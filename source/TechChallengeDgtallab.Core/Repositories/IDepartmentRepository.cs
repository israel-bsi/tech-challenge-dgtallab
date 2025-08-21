using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Repositories;

public interface IDepartmentRepository
{
    Task<Response<Department>> AddAsync(Department department);
    Task<Response<Department>> UpdateAsync(Department department);
    Task<Response<Department>> DeleteAsync(Department department);
    Task<Response<Department>> GetByIdAsync(int id);
    Task<PagedResponse<IEnumerable<Department>>> GetAllAsync(PagedRequest request);
}