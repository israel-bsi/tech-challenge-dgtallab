using TechChallengeDgtallab.ApiService.Services;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Handlers;

public class CollaboratorHandler : ICollaboratorHandler
{
    private readonly ICollaboratorRepository _collaboratorRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly CollaboratorService _collaboratorService;

    public CollaboratorHandler(ICollaboratorRepository collaboratorRepository,
        IDepartmentRepository departmentRepository,
        CollaboratorService collaboratorService)
    {
        _collaboratorRepository = collaboratorRepository;
        _departmentRepository = departmentRepository;
        _collaboratorService = collaboratorService;
    }

    public async Task<Response<CollaboratorResponse>> AddAsync(CreateCollaboratorRequest request)
    {
        try
        {
            var departmentResult = await _departmentRepository.GetByIdAsync(request.DepartmentId, true);
            if (departmentResult.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Departamento não encontrado.");

            var isCpfExists = await _collaboratorRepository.GetByCpfAsync(request.Cpf);
            if (isCpfExists.Data is not null)
                return new Response<CollaboratorResponse>(null, 409, "CPF já cadastrado.");

            if (!string.IsNullOrEmpty(request.Rg))
            {
                var isRgExists = await _collaboratorRepository.GetByRgAsync(request.Rg);
                if (isRgExists.Data is not null)
                    return new Response<CollaboratorResponse>(null, 409, "RG já cadastrado.");
            }

            var entity = new Collaborator
            {
                Name = request.Name,
                Cpf = request.Cpf,
                Rg = request.Rg,
                DepartmentId = request.DepartmentId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var result = await _collaboratorRepository.AddAsync(entity);
            return result.IsSuccess
                ? new Response<CollaboratorResponse>(result.Data?.ToResponse(), 201, "Colaborador cadastrado com sucesso!")
                : new Response<CollaboratorResponse>(null, 500, "Erro ao cadastrar colaborador.");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> UpdateAsync(UpdateCollaboratorRequest request)
    {
        try
        {
            var collaborator = await _collaboratorRepository.GetByIdAsync(request.Id, false);
            if (collaborator.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            var departmentResult = await _departmentRepository.GetByIdAsync(request.DepartmentId, false);
            if (departmentResult.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Departamento não encontrado.");

            if (collaborator.Data.Cpf != request.Cpf)
            {
                var isCpfExists = await _collaboratorRepository.GetByCpfAsync(request.Cpf);
                if (isCpfExists.Data is not null)
                    return new Response<CollaboratorResponse>(null, 409, "CPF já cadastrado.");
            }

            if (!string.IsNullOrEmpty(request.Rg) && collaborator.Data.Rg != request.Rg)
            {
                var isRgExists = await _collaboratorRepository.GetByRgAsync(request.Rg);
                if (isRgExists.Data is not null)
                    return new Response<CollaboratorResponse>(null, 409, "RG já cadastrado.");
            }

            var originalDepartmentId = collaborator.Data.DepartmentId;

            collaborator.Data.Name = request.Name;
            collaborator.Data.Cpf = request.Cpf;
            collaborator.Data.Rg = request.Rg;
            collaborator.Data.DepartmentId = request.DepartmentId;
            collaborator.Data.UpdatedAt = DateTime.UtcNow;

            if (originalDepartmentId != collaborator.Data.DepartmentId)
            {
                var oldManagedDepartment = await _departmentRepository.GetByManagerIdAsync(collaborator.Data.Id);

                if (oldManagedDepartment.Data is not null) 
                    oldManagedDepartment.Data.ManagerId = null;
            }

            var result = await _collaboratorRepository.UpdateAsync(collaborator.Data);
            return result.IsSuccess ? 
                new Response<CollaboratorResponse>(result.Data?.ToResponse(), 200, "Colaborador atualizado com sucesso!") :
                new Response<CollaboratorResponse>(null, 500, "Erro ao atualizar colaborador.");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _collaboratorRepository.GetByIdAsync(id, false);
            if (entity.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            entity.Data.IsActive = false;
            entity.Data.UpdatedAt = DateTime.UtcNow;

            await _collaboratorRepository.UpdateAsync(entity.Data);
            return new Response<CollaboratorResponse>(null, 204, "Colaborador deletado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _collaboratorRepository.GetByIdAsync(id, true);

            return entity.Data is null
                ? new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.")
                : new Response<CollaboratorResponse>(entity.Data.ToResponse(), 200, "Colaborador encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<IEnumerable<CollaboratorResponse>>> GetSubordinatesAsync(int managerId)
    {
        try
        {
            var managerResult = await _collaboratorRepository.GetByIdAsync(managerId, true);
            if (managerResult.Data == null)
                return new Response<IEnumerable<CollaboratorResponse>>(null, 404, "Gerente não encontrado.");

            var departmentsResult = await _collaboratorRepository.GetAllDepartmentsAsync();
            if (departmentsResult.Data == null)
                return new Response<IEnumerable<CollaboratorResponse>>(null, 200, "Nenhum departamento encontrado.");

            var allDepartments = departmentsResult.Data.ToList();

            var managedDepartmentIds = new List<int>();
            _collaboratorService.GetManagedDepartments(managerId, allDepartments, managedDepartmentIds);

            if (!managedDepartmentIds.Any())
                return new Response<IEnumerable<CollaboratorResponse>>(null, 200, "Nenhum colaborador subordinado encontrado.");

            var collaboratorsResult = await _collaboratorRepository.GetCollaboratorsByDepartmentIdsAsync(managedDepartmentIds, managerId);
            if (collaboratorsResult.Data == null)
                return new Response<IEnumerable<CollaboratorResponse>>(null, 200, "Nenhum colaborador subordinado encontrado.");

            var response = collaboratorsResult.Data.Select(c => c.ToResponse());
            return new Response<IEnumerable<CollaboratorResponse>>(response, 200, "Colaboradores subordinados encontrados com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<IEnumerable<CollaboratorResponse>>(null, 500, e.Message);
        }
    }

    public async Task<Response<IEnumerable<CollaboratorResponse>>> GetCollaboratorsByDepartment(int departmentId)
    {
        try
        {
            var departmentResult = await _departmentRepository.GetByIdAsync(departmentId, true);
            if (departmentResult.Data is null)
                return new Response<IEnumerable<CollaboratorResponse>>(null, 404, "Departamento não encontrado.");

            var collaboratorsResult = await _collaboratorRepository.GetCollaboratorsByDepartment(departmentId);
            if (collaboratorsResult.Data is null || !collaboratorsResult.Data.Any())
                return new Response<IEnumerable<CollaboratorResponse>>(null, 200, "Nenhum colaborador encontrado para o departamento informado.");

            var response = collaboratorsResult.Data.Select(c => c.ToResponse());
            return new Response<IEnumerable<CollaboratorResponse>>(response, 200, "Colaboradores encontrados com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<IEnumerable<CollaboratorResponse>>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request)
    {
        try
        {
            var result = await _collaboratorRepository.GetAllAsync(request);

            var response = result.Data?.Select(c => c.ToResponse());

            return new PagedResponse<IEnumerable<CollaboratorResponse>>(response, result.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<CollaboratorResponse>>(null, 500, e.Message);
        }
    }
}