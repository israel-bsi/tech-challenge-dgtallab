using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;
using TechChallengeDgtallab.Infra.Data;

namespace TechChallengeDgtallab.Infra.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _dbContext;

    public DepartmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<DepartmentResponse>> AddAsync(EditDepartmentRequest request)
    {
        try
        {
            var department = new Department
            {
                Name = request.Name,
                SuperiorDepartmentId = request.SuperiorDepartmentId,
                ManagerId = request.ManagerId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();

            return new Response<DepartmentResponse>(department, 201, "Departamento cadastrado com sucesso!");
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
            var department = await _dbContext
                .Departments
                .FirstOrDefaultAsync(d => d.Id == request.Id);
            if (department is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            department.Name = request.Name;
            department.ManagerId = request.ManagerId;
            department.SuperiorDepartmentId = request.SuperiorDepartmentId;
            department.UpdatedAt = DateTime.UtcNow;

            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();

            return new Response<DepartmentResponse>(department, 200, "Departamento atualizado com sucesso!");
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
            var department = await _dbContext
                .Departments
                .FirstOrDefaultAsync(d => d.Id == id);
            if (department is null)
                return new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.");

            department.IsActive = false;
            department.UpdatedAt = DateTime.UtcNow;

            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();

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
            var department = await _dbContext
                .Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            return department is null 
                ? new Response<DepartmentResponse>(null, 404, "Departamento não encontrado.") 
                : new Response<DepartmentResponse>(department, 200, "Departamento encontrado com sucesso!");
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
            var query = _dbContext
                .Departments
                .AsNoTracking()
                .Where(d => d.IsActive);

            var count = await query.CountAsync();

            var departments = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var departmentsResponse = departments.Select(c => new DepartmentResponse
            {
                Id = c.Id,
                Name = c.Name,
                SuperiorDepartment = c.SuperiorDepartment ?? new DepartmentResponse(),
                Manager = c.Manager ?? new CollaboratorResponse()
            });

            return new PagedResponse<IEnumerable<DepartmentResponse>>(departmentsResponse, count, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<DepartmentResponse>>(null, 500, e.Message);
        }
    }
}