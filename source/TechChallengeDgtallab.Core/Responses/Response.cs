using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Responses;

public class Response<TData>
{
    [JsonConstructor]
    public Response() => StatusCode = Configuration.DefaultStatusCode;

    public Response(TData? data, int statusCode = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }

    [JsonIgnore]
    public int StatusCode { get; set; }
    public TData? Data { get; set; }
    public string? Message { get; set; }

    [JsonIgnore]
    public bool IsSuccess => StatusCode is >= 200 and < 299;
}