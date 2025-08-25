using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Requests;

public class EditDepartmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public EditCollaboratorRequest? Manager { get; set; }
    public int? ManagerId { get; set; }

    [JsonIgnore]
    public EditDepartmentRequest? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }
}