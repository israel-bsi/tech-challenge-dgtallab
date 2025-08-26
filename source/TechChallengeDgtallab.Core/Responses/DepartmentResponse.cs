using TechChallengeDgtallab.Core.DTOs;

namespace TechChallengeDgtallab.Core.Responses;

public class DepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CollaboratorDto? Manager { get; set; }
    public DepartmentDto? SuperiorDepartment { get; set; }
}