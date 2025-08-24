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

    public async Task<Response<Department>> AddAsync(Department department)
    {
        await _dbContext.Departments.AddAsync(department);
        await _dbContext.SaveChangesAsync();

        await _dbContext.Entry(department).Reference(d => d.Manager).LoadAsync();
        await _dbContext.Entry(department).Reference(d => d.SuperiorDepartment).LoadAsync();

        return new Response<Department>(department);
    }

    public async Task<Response<Department>> UpdateAsync(Department department)
    {
        var existingEntity = _dbContext.ChangeTracker.Entries<Department>()
            .FirstOrDefault(e => e.Entity.Id == department.Id);

        if (existingEntity != null)
            existingEntity.CurrentValues.SetValues(department);
        else
            _dbContext.Departments.Update(department);

        await _dbContext.SaveChangesAsync();

        department = await _dbContext
            .Departments
            .Include(d => d.Manager)
            .Include(d => d.SuperiorDepartment)
            .AsNoTracking()
            .FirstAsync(d => d.Id == department.Id);

        return new Response<Department>(department);
    }

    public async Task<Response<Department>> DeleteAsync(Department department)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();

        return new Response<Department>();
    }

    public async Task<Response<Department>> GetByIdAsync(int id)
    {
        var department = await _dbContext
            .Departments
            .Include(c => c.Manager)
            .Include(c => c.SuperiorDepartment)
            .ThenInclude(d => d.Manager)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

        return new Response<Department>(department);
    }

    public async Task<(Response<List<Department>>, Response<Department>)> GetDepartmentHierarchyAsync(int id)
    {
        var rootDepartment = await _dbContext
            .Departments
            .Include(d => d.Manager)
            .Include(d => d.SuperiorDepartment)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);

        if (rootDepartment == null)
            return (new Response<List<Department>>(null), new Response<Department>(null));

        var allDepartments = await _dbContext
            .Departments
            .Include(d => d.Manager)
            .Include(d => d.SuperiorDepartment)
            .AsNoTracking()
            .Where(d => d.IsActive)
            .ToListAsync();

        return (new Response<List<Department>>(allDepartments), new Response<Department>(rootDepartment));
    }

    public async Task<PagedResponse<IEnumerable<Department>>> GetAllAsync(PagedRequest request)
    {
        var query = _dbContext
            .Departments
            .Include(c => c.Manager)
            .Include(c => c.SuperiorDepartment)
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderByDescending(d => d.CreatedAt);

        var count = await query.CountAsync();

        var departments = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResponse<IEnumerable<Department>>(departments, count, request.PageNumber, request.PageSize);
    }
}