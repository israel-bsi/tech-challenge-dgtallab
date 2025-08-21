using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Responses;

public class ErrorData
{
    [JsonIgnore]
    public int HttpStatusCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Error> Errors { get; set; } = [];

    [JsonConstructor]
    public ErrorData() { }

    public ErrorData(int httpStatusCode = 400, 
        string? description = "Requisição inválida", 
        List<Error>? errors = null)
    {
        HttpStatusCode = httpStatusCode;
        Description = description ?? string.Empty;
        Errors = errors ?? [];
    }
}

public class Error
{
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}