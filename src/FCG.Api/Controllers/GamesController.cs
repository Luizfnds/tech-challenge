using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FCG.Application.Commands.Games.CreateGame;
using FCG.Application.Commands.Games.UpdateGame;
using FCG.Application.Commands.Games.ActivateGame;
using FCG.Application.Commands.Games.DeactivateGame;
using FCG.Application.Queries.Games.GetAllGames;
using FCG.Application.Queries.Games.GetGameById;
using FCG.Application.Queries.Games.GetActiveGames;
using FCG.API.Extensions;
using FCG.API.Contracts.Responses;
using FCG.API.Contracts.Requests;
using FCG.Domain.Entities;
using FCG.Application.Commands.Games.PurchaseGame;
using FCG.Application.Queries.Games.GetUserGames;

namespace FCG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GamePagedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetGames(
        [FromQuery] bool active = true, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        // Se active for false, requer privilégio de admin
        if (!active && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var result = active
            ? await _mediator.Send(new GetActiveGamesQuery(pageNumber, pageSize))
            : await _mediator.Send(new GetAllGamesQuery(pageNumber, pageSize));

        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetGameByIdQuery(id);
        var result = await _mediator.Send(query);

        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(CreateGameResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateGameCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            new { gameId = result.Value, message = "Game created successfully." });
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] bool active)
    {
        var result = active
            ? await _mediator.Send(new ActivateGameCommand(id))
            : await _mediator.Send(new DeactivateGameCommand(id));

        if (result.IsFailure)
            return result.ToActionResult();

        var message = active ? "Game activated successfully." : "Game deactivated successfully.";
        return Ok(new { message });
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameRequest dto)
    {
        var command = new UpdateGameCommand(
            id,
            dto.Title,
            dto.Description,
            dto.Genre,
            dto.Price,
            dto.ReleaseDate,
            dto.Publisher
        );

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        return Ok(new { message = "Game updated successfully." });
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(UserGamePagedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserGames(
        [FromRoute] Guid userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 100) pageSize = 100;
        if (pageNumber < 1) pageNumber = 1;

        var query = new GetUserGamesQuery(userId, pageNumber, pageSize);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return result.ToActionResult();

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(ug => new UserGameItemResponse(
            ug.Id,
            ug.GameId,
            ug.Game.Title,
            ug.Game.Description,
            ug.Game.Genre,
            ug.Game.Publisher,
            ug.PurchaseDate,
            ug.PurchasePrice
        ));

        var response = new UserGamePagedResponse(
            items,
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalCount,
            pagedResult.TotalPages,
            pagedResult.HasPreviousPage,
            pagedResult.HasNextPage
        );

        return Ok(response);
    }

    [HttpPost("user/{userId:guid}/purchase")]
    [ProducesResponseType(typeof(PurchaseGameResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PurchaseGame(
        [FromRoute] Guid userId,
        [FromBody] PurchaseGameRequest request)
    {
        var command = new PurchaseGameCommand(userId, request.GameId);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        var response = new PurchaseGameResponse(
            result.Value,
            "Game successfully added to your library"
        );

        return CreatedAtAction(
            nameof(GetUserGames),
            new { userId },
            response);
    }
}
