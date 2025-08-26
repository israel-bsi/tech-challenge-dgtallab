using Microsoft.AspNetCore.Mvc.ModelBinding;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Extensions;

public static class ModelStateExtensions
{
    public static ErrorData CreateErrorResponse(
        this ModelStateDictionary modelState,
        int statusCode = 400,
        string? title = null)
    {
        var errors = new List<Error>();

        foreach (var kvp in modelState)
        {
            var fieldName = kvp.Key;
            var modelStateEntry = kvp.Value;

            if (modelStateEntry.Errors.Count > 0)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    errors.Add(new Error
                    {
                        Field = fieldName,
                        Message = !string.IsNullOrEmpty(error.ErrorMessage)
                            ? error.ErrorMessage
                            : "Erro de validação"
                    });
                }
            }
        }

        return new ErrorData
        {
            HttpStatusCode = statusCode,
            Description = title ?? "Requisição inválida",
            Errors = errors
        };
    }
}