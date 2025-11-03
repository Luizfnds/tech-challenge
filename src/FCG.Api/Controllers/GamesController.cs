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
using FCG.API.DTOs;
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
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(GamePagedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllGamesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        return result.ToActionResult();
    }

    [HttpGet("active")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GamePagedResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveGames([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetActiveGamesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

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

    [HttpPost("{id:guid}/activate")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var command = new ActivateGameCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        return Ok(new { message = "Game activated successfully." });
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivateGameCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        return Ok(new { message = "Game deactivated successfully." });
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameDto dto)
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

        var response = new UserGamePagedResponse
        {
            Items = pagedResult.Items.Select(ug => new UserGameItemResponse
            {
                Id = ug.Id,
                GameId = ug.GameId,
                GameTitle = ug.Game.Title,
                GameDescription = ug.Game.Description,
                GameGenre = ug.Game.Genre,
                GamePublisher = ug.Game.Publisher,
                PurchaseDate = ug.PurchaseDate,
                PurchasePrice = ug.PurchasePrice
            }),
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages,
            HasPreviousPage = pagedResult.HasPreviousPage,
            HasNextPage = pagedResult.HasNextPage
        };

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

        var response = new PurchaseGameResponse
        {
            UserGameId = result.Value,
            Message = "Game successfully added to your library"
        };

        return CreatedAtAction(
            nameof(GetUserGames),
            new { userId },
            response);
    }
}

public record PurchaseGameRequest
{
    public Guid GameId { get; init; }
}
