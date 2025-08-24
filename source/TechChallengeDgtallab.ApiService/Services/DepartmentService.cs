using TechChallengeDgtallab.Core.Models;

namespace TechChallengeDgtallab.ApiService.Services;

public class DepartmentService
{
    public void GetSubordinateDepartments(int parentId, List<Department> allDepartments, List<Department> result)
    {
        var subordinates = allDepartments.Where(d => d.SuperiorDepartmentId == parentId).ToList();

        foreach (var subordinate in subordinates)
        {
            result.Add(subordinate);

            GetSubordinateDepartments(subordinate.Id, allDepartments, result);
        }
    }
}