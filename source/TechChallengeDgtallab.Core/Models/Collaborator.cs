namespace TechChallengeDgtallab.Core.Models;

public class Collaborator : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Rg { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = new();
}