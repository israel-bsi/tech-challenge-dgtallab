using Microsoft.AspNetCore.Mvc;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Extensions;

public static class ControllerExtension
{
    public static ActionResult ToActionResult(
        this ControllerBase controller, ErrorData response)
    {
        return response.HttpStatusCode switch
        {
            404 => controller.NotFound(response),
            500 => controller.StatusCode(500, response),
            _ => controller.BadRequest(response)
        };
    }
}