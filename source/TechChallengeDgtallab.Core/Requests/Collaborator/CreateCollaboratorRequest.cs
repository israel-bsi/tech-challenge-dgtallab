using System.ComponentModel.DataAnnotations;
using TechChallengeDgtallab.Core.DTOs;

namespace TechChallengeDgtallab.Core.Requests.Collaborator;

public class CreateCollaboratorRequest
{

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório")]
    public string Cpf { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
    public int DepartmentId { get; set; }

    public DepartmentDto Department { get; set; } = new();
    public string? Rg { get; set; }
}