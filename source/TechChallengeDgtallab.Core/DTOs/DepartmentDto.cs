namespace TechChallengeDgtallab.Core.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
    public int? ManagerId { get; set; }
    public string SuperiorDepartment { get; set; } = string.Empty;
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}