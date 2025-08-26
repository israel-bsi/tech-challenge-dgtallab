using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Department;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Handler;

public interface IDepartmentHandler
{
    Task<Response<DepartmentResponse>> AddAsync(CreateDepartmentRequest request);
    Task<Response<DepartmentResponse>> UpdateAsync(UpdateDepartmentRequest request);
    Task<Response<DepartmentResponse>> DeleteAsync(int id);
    Task<Response<DepartmentResponse>> GetByIdAsync(int id);
    Task<Response<IEnumerable<DepartmentResponse>>> GetDepartmentHierarchyAsync(int id);
    Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request);
}