using Microsoft.AspNetCore.Mvc;
using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Controllers;

[ApiController]
[Tags("Colaboradores")]
[Route("api/v1/department")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    [HttpPost]
    [EndpointSummary("Cria um novo departamento")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddAsync([FromBody] EditDepartmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var response = await _departmentRepository.AddAsync(request);

        return response.IsSuccess
            ? CreatedAtRoute(nameof(AddAsync), new { id = response.Data?.Id }, response.Data)
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }
}