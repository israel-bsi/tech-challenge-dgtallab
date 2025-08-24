using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Requests;

public class EditDepartmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public EditCollaboratorRequest? CollaboratorRequest { get; set; }
    public int? ManagerId { get; set; }

    [JsonIgnore]
    public EditDepartmentRequest? SuperiorDepartmentRequest { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    [JsonIgnore]
    public bool IsActive { get; set; }
}