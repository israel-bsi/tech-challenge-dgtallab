using System.Text.Json.Serialization;

namespace TechChallengeDgtallab.Core.Responses;

public class Response<TData>
{
    [JsonConstructor]
    public Response() => Code = Configuration.DefaultStatusCode;

    public Response(TData? data, int code = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        Code = code;
        Message = message;
    }

    public int Code { get; set; }
    public TData? Data { get; set; }
    public string? Message { get; set; }

    [JsonIgnore]
    public bool IsSuccess => Code is >= 200 and < 299;
}