using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Handlers;

public class DepartmentHandler : IDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public DepartmentHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<DepartmentResponse>> AddAsync(EditDepartmentRequest request)
    {
        try
        {
            var entity = new Department();
            entity.Name = request.Name;
            entity.SuperiorDepartmentId = request.SuperiorDepartmentId;
            entity.ManagerId = request.ManagerId;
            entity.IsActive = true;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.AddAsync(entity);
            return new Response<DepartmentResponse>(entity, 201, "Departamento cadastrado com sucesso!");
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
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity.Data is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            entity.Data.Name = request.Name;
            entity.Data.ManagerId = request.ManagerId;
            entity.Data.SuperiorDepartmentId = request.SuperiorDepartmentId;
            entity.Data.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity.Data);
            return new Response<DepartmentResponse>(entity.Data, 200, "Departamento atualizado com sucesso!");
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
            var entity = await _repository.GetByIdAsync(id);
            if (entity.Data is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            entity.Data.IsActive = false;
            entity.Data.UpdatedAt = DateTime.UtcNow;

            await _repository.DeleteAsync(entity.Data);

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
            var entity = await _repository.GetByIdAsync(id);

            return entity.Data is null
                ? new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.")
                : new Response<DepartmentResponse>(entity.Data, 200, "Departamento encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<DepartmentResponse>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request)
    {
        try
        {
            var response = await _repository.GetAllAsync(request);
            if (response.Data is null || !response.Data.Any())
                return new PagedResponse<IEnumerable<DepartmentResponse>>(null, 404, "Nenhum departamento encontrado.");

            var departmentsResponse = response.Data.Select(c => new DepartmentResponse
            {
                Id = c.Id,
                Name = c.Name,
                SuperiorDepartment = c.SuperiorDepartment ?? new DepartmentResponse(),
                Manager = c.Manager ?? new CollaboratorResponse()
            });

            return new PagedResponse<IEnumerable<DepartmentResponse>>(departmentsResponse, response.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<DepartmentResponse>>(null, 500, e.Message);
        }
    }
}