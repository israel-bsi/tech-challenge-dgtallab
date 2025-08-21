using System.ComponentModel.DataAnnotations;

namespace TechChallengeDgtallab.Core.Requests;

public class EditCollaboratorRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "O Departamento é obrigatório")]
    public int DepartmentId { get; set; }
    public string? Rg { get; set; }
}