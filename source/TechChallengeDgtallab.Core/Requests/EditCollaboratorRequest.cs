using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Requests;

public class EditCollaboratorRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório")]
    public string Cpf { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
    public int DepartmentId { get; set; }

    [JsonIgnore]
    public EditDepartmentRequest? DepartmentRequest { get; set; }
    public string? Rg { get; set; }
}