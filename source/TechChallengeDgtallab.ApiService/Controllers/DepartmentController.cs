using Microsoft.AspNetCore.Mvc;
using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.Core;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Department;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Controllers;

[ApiController]
[Tags("Departamentos")]
[Route("api/v1/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentHandler _handler;

    public DepartmentController(IDepartmentHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    [EndpointSummary("Cria um novo departamento")]
    [ProducesResponseType(typeof(Response<DepartmentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddAsync([FromBody] CreateDepartmentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse());

        var response = await _handler.AddAsync(request);

        return response.IsSuccess
            ? CreatedAtRoute("GetDepartmentByIdAsync", new { id = response.Data?.Id }, response.Data)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Atualiza um departamento")]
    [ProducesResponseType(typeof(Response<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateDepartmentRequest request)
    {
        request.Id = id;
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse());
    
        var response = await _handler.UpdateAsync(request);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet("{id:int}", Name = "GetDepartmentByIdAsync")]
    [EndpointSummary("Obtém um departamento pelo ID")]
    [ProducesResponseType(typeof(Response<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _handler.GetByIdAsync(id);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet("hierarchy/{id:int}")]
    [EndpointSummary("Obtém a hierarquia de departamentos pelo ID")]
    [ProducesResponseType(typeof(Response<List<DepartmentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetDepartmentHierarchyAsync([FromRoute] int id)
    {
        var response = await _handler.GetDepartmentHierarchyAsync(id);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Deleta um departamento pelo ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var response = await _handler.DeleteAsync(id);

        return response.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet]
    [EndpointSummary("Obtém todos os departamentos")]
    [ProducesResponseType(typeof(PagedResponse<IEnumerable<DepartmentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize,
        [FromQuery] string searchTerm = "",
        [FromQuery] string filterBy = "")
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse());

        var request = new PagedRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            FilterBy = filterBy,
            SearchTerm = searchTerm
        };
        var response = await _handler.GetAllAsync(request);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }
}