using System.Net;
using System.Net.Http.Json;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<Response<T>> ProcessResponseAsync<T>(this HttpResponseMessage response)
    {
        try
        {
            if (response.StatusCode is HttpStatusCode.NoContent)
                return new Response<T>();

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<Response<T>>() ?? new Response<T>();

            var errorData = await response.Content.ReadFromJsonAsync<ErrorData>();
            return new Response<T>(default, errorData!.HttpStatusCode, errorData.Description);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<PagedResponse<T>> ProcessPagedResponseAsync<T>(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return (await response.Content.ReadFromJsonAsync<PagedResponse<T>>())!;

        var errorData = await response.Content.ReadFromJsonAsync<ErrorData>();
        return new PagedResponse<T>(default, errorData!.HttpStatusCode, errorData.Description);
    }
}