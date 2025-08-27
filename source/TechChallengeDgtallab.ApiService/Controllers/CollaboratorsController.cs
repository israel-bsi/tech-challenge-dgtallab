using Microsoft.AspNetCore.Mvc;
using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.Core;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.ApiService.Controllers;

[ApiController]
[Tags("Colaboradores")]
[Route("api/v1/collaborators")]
public class CollaboratorsController : ControllerBase
{
    private readonly ICollaboratorHandler _handler;
    public CollaboratorsController(ICollaboratorHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    [EndpointSummary("Cria um novo colaborador")]
    [ProducesResponseType(typeof(Response<CollaboratorResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddAsync([FromBody] CreateCollaboratorRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse());

        var response = await _handler.AddAsync(request);

        return response.IsSuccess
            ? CreatedAtRoute("GetCollaboratorByIdAsync", new { id = response.Data?.Id }, response.Data)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Atualiza um colaborador")]
    [ProducesResponseType(typeof(Response<CollaboratorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateCollaboratorRequest request)
    {
        request.Id = id;
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse());

        var response = await _handler.UpdateAsync(request);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet("{id:int}", Name = "GetCollaboratorByIdAsync")]
    [EndpointSummary("Obtém um colaborador pelo ID")]
    [ProducesResponseType(typeof(Response<CollaboratorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _handler.GetByIdAsync(id);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Deleta um colaborador pelo ID")]
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

    [HttpGet("subordinates/{managerId:int}")]
    [EndpointSummary("Obtém todos os colaboradores subordinados a um gerente")]
    [ProducesResponseType(typeof(Response<IEnumerable<CollaboratorResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSubordinateCollaboratorsAsync([FromRoute] int managerId)
    {
        var response = await _handler.GetSubordinatesAsync(managerId);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet("department/{id:int}")]
    [EndpointSummary("Obtém todos os colaboradores de um departamento")]
    [ProducesResponseType(typeof(Response<IEnumerable<CollaboratorResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetCollaboratorsByDepartmentAsync([FromRoute] int id)
    {
        var response = await _handler.GetCollaboratorsByDepartment(id);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.StatusCode, response.Message));
    }

    [HttpGet]
    [EndpointSummary("Obtém todos os colaboradores")]
    [ProducesResponseType(typeof(PagedResponse<IEnumerable<CollaboratorResponse>>), StatusCodes.Status200OK)]
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