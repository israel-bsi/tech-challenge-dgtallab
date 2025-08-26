using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;
using TechChallengeDgtallab.Infra.Data;

namespace TechChallengeDgtallab.Infra.Repositories;

public class CollaboratorsRepository : ICollaboratorRepository
{
    private readonly AppDbContext _dbContext;
    public CollaboratorsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<Collaborator>> AddAsync(Collaborator collaborator)
    {
        var department = await _dbContext
            .Departments
            .FirstAsync(d => d.Id == collaborator.DepartmentId && d.IsActive);

        _dbContext.Attach(department);
        collaborator.Department = department;

        await _dbContext.Collaborators.AddAsync(collaborator);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(collaborator).Reference(c => c.Department).LoadAsync();

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<Collaborator>> UpdateAsync(Collaborator request)
    {
        var department = await _dbContext
            .Departments
            .FirstAsync(d => d.Id == request.DepartmentId && d.IsActive);

        _dbContext.Attach(department);
        request.Department = department;

        var existingEntity = _dbContext.ChangeTracker.Entries<Collaborator>()
            .FirstOrDefault(e => e.Entity.Id == request.Id);

        if (existingEntity != null)
            existingEntity.CurrentValues.SetValues(request);
        else
            _dbContext.Collaborators.Update(request);

        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(request).Reference(c => c.Department).LoadAsync();

        return new Response<Collaborator>(request);
    }

    public async Task<Response<Collaborator>> DeleteAsync(Collaborator collaborator)
    {
        _dbContext.Collaborators.Update(collaborator);
        await _dbContext.SaveChangesAsync();

        return new Response<Collaborator>();
    }

    public async Task<Response<Collaborator>> GetByIdAsync(int id)
    {
        var collaborator = await _dbContext
            .Collaborators
            .Include(c => c.Department)
            .ThenInclude(d => d.Manager)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<Collaborator>> GetByCpfAsync(string cpf)
    {
        var collaborator = await _dbContext
            .Collaborators
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Cpf == cpf && c.IsActive);

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<Collaborator>> GetByRgAsync(string? rg)
    {
        var collaborator = await _dbContext
            .Collaborators
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Rg == rg && c.IsActive);

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<IEnumerable<Collaborator>>> GetCollaboratorsByDepartment(int departmentId)
    {
        var collaborators = await _dbContext
            .Collaborators
            .Include(c => c.Department)
            .ThenInclude(d => d.Manager)
            .AsNoTracking()
            .Where(c => c.IsActive && c.DepartmentId == departmentId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new Response<IEnumerable<Collaborator>>(collaborators);
    }

    public async Task<PagedResponse<IEnumerable<Collaborator>>> GetAllAsync(PagedRequest request)
    {
        var query = _dbContext
            .Collaborators
            .Include(c => c.Department)
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt);

        var count = await query.CountAsync();

        var collaborators = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResponse<IEnumerable<Collaborator>>(collaborators, count, request.PageNumber, request.PageSize);
    }

    public async Task<Response<IEnumerable<Department>>> GetAllDepartmentsAsync()
    {
        var departments = await _dbContext
            .Departments
            .Include(d => d.Manager)
            .AsNoTracking()
            .Where(d => d.IsActive)
            .ToListAsync();

        return new Response<IEnumerable<Department>>(departments);
    }

    public async Task<Response<IEnumerable<Collaborator>>> GetCollaboratorsByDepartmentIdsAsync(List<int> departmentIds, int excludeManagerId)
    {
        var collaborators = await _dbContext
            .Collaborators
            .Include(c => c.Department)
            .ThenInclude(d => d.Manager)
            .AsNoTracking()
            .Where(c => c.IsActive &&
                       departmentIds.Contains(c.DepartmentId) &&
                       c.Id != excludeManagerId)
            .OrderBy(c => c.Department.Name)
            .ThenBy(c => c.Name)
            .ToListAsync();

        return new Response<IEnumerable<Collaborator>>(collaborators);
    }
}