using Microsoft.AspNetCore.Mvc;
using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
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
    [ProducesResponseType(typeof(CollaboratorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddAsync([FromBody] EditCollaboratorRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var response = await _handler.AddAsync(request);

        return response.IsSuccess
            ? CreatedAtRoute(nameof(AddAsync), new { id = response.Data?.Id }, response.Data)
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }

    [HttpPut]
    [EndpointSummary("Atualiza um colaborador")]
    [ProducesResponseType(typeof(CollaboratorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateAsync([FromBody] EditCollaboratorRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var response = await _handler.UpdateAsync(request);

        return response.IsSuccess
            ? Ok(response.Data)
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }


    [HttpGet("{id:int}")]
    [EndpointSummary("Obtém um colaborador pelo ID")]
    [ProducesResponseType(typeof(CollaboratorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int id)
    {
        var response = await _handler.GetByIdAsync(id);

        return response.IsSuccess
            ? Ok(response.Data)
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Deleta um colaborador pelo ID")]
    [ProducesResponseType(typeof(CollaboratorResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var response = await _handler.DeleteAsync(id);

        return response.IsSuccess
            ? NoContent()
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }

    [HttpGet]
    [EndpointSummary("Obtém todos os colaboradores")]
    [ProducesResponseType(typeof(PagedResponse<IEnumerable<CollaboratorResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllAsync([FromQuery] PagedRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.CreateErrorResponse(request));

        var response = await _handler.GetAllAsync(request);

        return response.IsSuccess
            ? Ok(response)
            : this.ToActionResult(new ErrorData(response.Code, response.Message));
    }
}