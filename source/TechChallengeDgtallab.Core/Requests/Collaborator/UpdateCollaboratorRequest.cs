using System.ComponentModel.DataAnnotations;
using TechChallengeDgtallab.Core.Requests.Department;

namespace TechChallengeDgtallab.Core.Requests.Collaborator;

public class UpdateCollaboratorRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório")]
    public string Cpf { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
    public int DepartmentId { get; set; }

    public DepartmentDto? Department { get; set; }
    public string? Rg { get; set; }

    public override string ToString() => Name;
}