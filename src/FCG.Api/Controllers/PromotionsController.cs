using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FCG.Application.Commands.Promotions.CreatePromotion;
using FCG.Application.Commands.Promotions.DeactivatePromotion;
using FCG.Application.Queries.Promotions.GetAllPromotions;
using FCG.Application.Queries.Promotions.GetPromotionById;
using FCG.API.Extensions;
using FCG.API.Contracts.Responses;

namespace FCG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class PromotionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PromotionPagedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 100) pageSize = 100;
        if (pageNumber < 1) pageNumber = 1;

        var query = new GetAllPromotionsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return result.ToActionResult();

        var pagedResult = result.Value;

        var items = pagedResult.Items.Select(p => new PromotionItemResponse(
            p.Id,
            p.GameId,
            p.Game.Title,
            p.DiscountPercentage,
            p.StartDate,
            p.EndDate,
            p.IsActive,
            p.IsValid(),
            p.CreatedAt
        ));

        var response = new PromotionPagedResponse(
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PromotionItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetPromotionByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return result.ToActionResult();

        var promotion = result.Value;

        var response = new PromotionItemResponse(
            promotion.Id,
            promotion.GameId,
            promotion.Game.Title,
            promotion.DiscountPercentage,
            promotion.StartDate,
            promotion.EndDate,
            promotion.IsActive,
            promotion.IsValid(),
            promotion.CreatedAt
        );

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatePromotionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreatePromotionCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        var response = new CreatePromotionResponse(
            result.Value,
            "Promotion created successfully."
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var command = new DeactivatePromotionCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return result.ToActionResult();

        return Ok(new MessageResponse("Promotion deactivated successfully."));
    }
}
