using TechChallengeDgtallab.Core.Models;

namespace TechChallengeDgtallab.Core.Responses;

public class DepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CollaboratorResponse Manager { get; set; } = new();
    public DepartmentResponse? SuperiorDepartment { get; set; }

    public static implicit operator DepartmentResponse(Department department)
    {
        return new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Manager = department.Manager ?? new CollaboratorResponse(),
            SuperiorDepartment = department.SuperiorDepartment ?? new DepartmentResponse()
        };
    }
}