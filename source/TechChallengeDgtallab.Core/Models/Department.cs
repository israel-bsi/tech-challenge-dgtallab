namespace TechChallengeDgtallab.Core.Models;

public class Department : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public int ManagerId { get; set; }
    public Collaborator? Manager { get; set; }
    public int SuperiorDepartmentId { get; set; }
    public Department? SuperiorDepartment { get; set; }
}