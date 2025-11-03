using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FCG.Application.Commands.Promotions.CreatePromotion;
using FCG.Application.Commands.Promotions.DeactivatePromotion;
using FCG.Application.Queries.Promotions.GetAllPromotions;
using FCG.Application.Queries.Promotions.GetPromotionById;
using FCG.API.Extensions;
using FCG.API.DTOs;

namespace FCG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class PromotionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PromotionsController> _logger;

    public PromotionsController(IMediator mediator, ILogger<PromotionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

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

        var response = new PromotionPagedResponse
        {
            Items = pagedResult.Items.Select(p => new PromotionItemResponse
            {
                Id = p.Id,
                GameId = p.GameId,
                GameTitle = p.Game.Title,
                DiscountPercentage = p.DiscountPercentage,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsActive = p.IsActive,
                IsValid = p.IsValid(),
                CreatedAt = p.CreatedAt
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

        var response = new PromotionItemResponse
        {
            Id = promotion.Id,
            GameId = promotion.GameId,
            GameTitle = promotion.Game.Title,
            DiscountPercentage = promotion.DiscountPercentage,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate,
            IsActive = promotion.IsActive,
            IsValid = promotion.IsValid(),
            CreatedAt = promotion.CreatedAt
        };

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

        var response = new CreatePromotionResponse
        {
            PromotionId = result.Value,
            Message = "Promotion created successfully."
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            response);
    }

    [HttpPost("{id:guid}/deactivate")]
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

        return Ok(new MessageResponse { Message = "Promotion deactivated successfully." });
    }
}
