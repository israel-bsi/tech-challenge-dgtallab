using TechChallengeDgtallab.Core.Models;

namespace TechChallengeDgtallab.ApiService.Services;

public class CollaboratorService
{
    public void GetManagedDepartments(int managerId, List<Department> allDepartments, List<int> result)
    {
        var directlyManagedDepartments = allDepartments
            .Where(d => d.ManagerId == managerId)
            .ToList();

        foreach (var department in directlyManagedDepartments)
        {
            if (!result.Contains(department.Id))
            {
                result.Add(department.Id);

                GetSubordinateDepartmentsForManager(department.Id, allDepartments, result);
            }
        }
    }

    private void GetSubordinateDepartmentsForManager(int parentDepartmentId, List<Department> allDepartments, List<int> result)
    {
        var subordinateDepartments = allDepartments
            .Where(d => d.SuperiorDepartmentId == parentDepartmentId)
            .ToList();

        foreach (var subordinate in subordinateDepartments)
        {
            if (!result.Contains(subordinate.Id))
            {
                result.Add(subordinate.Id);

                GetSubordinateDepartmentsForManager(subordinate.Id, allDepartments, result);
            }
        }
    }
}