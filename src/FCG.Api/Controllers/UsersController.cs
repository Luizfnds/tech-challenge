using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FCG.Application.Queries.GetUserById;
using FCG.API.Extensions;

namespace FCG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "UserOrAdmin")]
public class UsersController(IMediator mediator, ILogger<UsersController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<UsersController> _logger = logger;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.ToActionResult();
    }
}
