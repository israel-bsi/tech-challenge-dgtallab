namespace TechChallengeDgtallab.Core.DTOs;

public class CollaboratorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
    public int DepartmentId { get; set; }

    public override string ToString() => Name;
}