using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Handlers;

public class CollaboratorHandler : ICollaboratorHandler
{
    private readonly ICollaboratorRepository _repository;

    public CollaboratorHandler(ICollaboratorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<CollaboratorResponse>> AddAsync(EditCollaboratorRequest request)
    {
        try
        {
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

            await _repository.AddAsync(entity);
            return new Response<CollaboratorResponse>(entity, 201, "Colaborador cadastrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> UpdateAsync(EditCollaboratorRequest request)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            entity.Data.Name = request.Name;
            entity.Data.Cpf = request.Cpf;
            entity.Data.Rg = request.Rg;
            entity.Data.DepartmentId = request.DepartmentId;
            entity.Data.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity.Data);
            return new Response<CollaboratorResponse>(entity.Data, 200, "Colaborador atualizado com sucesso!");
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
            var entity = await _repository.GetByIdAsync(id);
            if (entity.Data is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            entity.Data.IsActive = false;
            entity.Data.UpdatedAt = DateTime.UtcNow;

            await _repository.DeleteAsync(entity.Data);
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
            var entity = await _repository.GetByIdAsync(id);

            return entity.Data is null 
                ? new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.") 
                : new Response<CollaboratorResponse>(entity.Data, 200, "Colaborador encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request)
    {
        try
        {
            var response = await _repository.GetAllAsync(request);
            if (response.Data is null || !response.Data.Any())
                return new PagedResponse<IEnumerable<CollaboratorResponse>>(null, 404, "Nenhum colaborador encontrado.");

            var collaboratorsResponse = response.Data?.Select(c => new CollaboratorResponse
            {
                Id = c.Id,
                Name = c.Name,
                Cpf = c.Cpf,
                Rg = c.Rg,
                Department = c.Department
            });

            return new PagedResponse<IEnumerable<CollaboratorResponse>>(collaboratorsResponse, response.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<CollaboratorResponse>>(null, 500, e.Message);
        }
    }
}