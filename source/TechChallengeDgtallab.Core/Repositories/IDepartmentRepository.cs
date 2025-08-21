using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Repositories;

public interface IDepartmentRepository
{
    Task<Response<DepartmentResponse>> AddAsync(EditDepartmentRequest request);
    Task<Response<DepartmentResponse>> UpdateAsync(EditDepartmentRequest request);
    Task<Response<DepartmentResponse>> DeleteAsync(int id);
    Task<Response<DepartmentResponse>> GetByIdAsync(int id);
    Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request);
}