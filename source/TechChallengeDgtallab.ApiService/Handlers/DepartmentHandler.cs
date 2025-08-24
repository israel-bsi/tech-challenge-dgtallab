using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.ApiService.Services;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Handlers;

public class DepartmentHandler : IDepartmentHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly DepartmentService _departmentService;

    public DepartmentHandler(IDepartmentRepository departmentRepository, 
        ICollaboratorRepository collaboratorRepository, 
        DepartmentService departmentService)
    {
        _departmentRepository = departmentRepository;
        _collaboratorRepository = collaboratorRepository;
        _departmentService = departmentService;
    }

    public async Task<Response<DepartmentResponse>> AddAsync(EditDepartmentRequest request)
    {
        try
        {
            var collaborator = await _collaboratorRepository.GetByIdAsync(request.ManagerId ?? 0);
            if (request.ManagerId.HasValue && collaborator.Data is null)
                return new Response<DepartmentResponse>(null, 404, "Gerente não encontrado.");

            if (request.ManagerId > 0 && collaborator.Data?.DepartmentId != request.Id)
                return new Response<DepartmentResponse>(null, 400, "O gerente deve pertencer ao departamento.");

            var department = new Department
            {
                Name = request.Name,
                SuperiorDepartmentId = request.SuperiorDepartmentId,
                ManagerId = request.ManagerId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _departmentRepository.AddAsync(department);
            return new Response<DepartmentResponse>(result.Data?.ToResponse(), 201, "Departamento cadastrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<DepartmentResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<DepartmentResponse>> UpdateAsync(EditDepartmentRequest request)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(request.Id);
            if (department.Data is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            if (request.ManagerId.HasValue)
            {
                var manager = await _collaboratorRepository.GetByIdAsync(request.ManagerId.Value);
                if (manager.Data is null)
                    return new Response<DepartmentResponse>(null, 404, "Gerente não encontrado.");

                if (manager.Data.DepartmentId != department.Data.Id)
                    return new Response<DepartmentResponse>(null, 400, "O gerente deve pertencer ao departamento.");
            }

            if (request.SuperiorDepartmentId.HasValue)
            {
                var superiorDepartment = await _departmentRepository.GetByIdAsync(request.SuperiorDepartmentId.Value);
                if (superiorDepartment.Data is null)
                    return new Response<DepartmentResponse>(null, 404, "Departamento superior não encontrado.");
            }

            department.Data = request.ToEntity();

            var result = await _departmentRepository.UpdateAsync(department.Data);
            return new Response<DepartmentResponse>(result.Data?.ToResponse(), 200, "Departamento atualizado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<DepartmentResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<DepartmentResponse>> DeleteAsync(int id)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department.Data is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            department.Data.IsActive = false;
            department.Data.UpdatedAt = DateTime.UtcNow;

            await _departmentRepository.DeleteAsync(department.Data);

            return new Response<DepartmentResponse>(null, 204, "Departamento excluído com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<DepartmentResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<DepartmentResponse>> GetByIdAsync(int id)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(id);

            return department.Data is null
                ? new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.")
                : new Response<DepartmentResponse>(department.Data.ToResponse(), 200, "Departamento encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<DepartmentResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<IEnumerable<DepartmentResponse>>> GetDepartmentHierarchyAsync(int id)
    {
        try
        {
            var departments = await _departmentRepository.GetDepartmentHierarchyAsync(id);

            var allDepartments = departments.Item1.Data ?? new List<Department>();
            var rootDepartment = departments.Item2.Data ?? new Department();

            var hierarchyList = new List<Department> { rootDepartment };
            _departmentService.GetSubordinateDepartments(id, allDepartments, hierarchyList);

            return new Response<IEnumerable<DepartmentResponse>>(hierarchyList.ToResponse());
        }
        catch (Exception e)
        {
            return new Response<IEnumerable<DepartmentResponse>>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request)
    {
        try
        {
            var departments = await _departmentRepository.GetAllAsync(request);

            var response = departments.Data?.ToResponse();

            return new PagedResponse<IEnumerable<DepartmentResponse>>(response, departments.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<DepartmentResponse>>(null, 500, e.Message);
        }
    }
}