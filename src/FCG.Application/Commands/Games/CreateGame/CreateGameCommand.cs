using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.Commands.Games.CreateGame;

/// <summary>
/// Command to create a new game
/// </summary>
public record CreateGameCommand(
    /// <summary>
    /// Game title (3-200 characters)
    /// </summary>
    /// <example>Cyberpunk 2077</example>
    string Title,
    
    /// <summary>
    /// Detailed game description (max 2000 characters)
    /// </summary>
    /// <example>An open-world action-adventure RPG set in Night City</example>
    string Description,
    
    /// <summary>
    /// Game genre (max 100 characters)
    /// </summary>
    /// <example>RPG</example>
    string Genre,
    
    /// <summary>
    /// Game price in USD (must be >= 0)
    /// </summary>
    /// <example>59.99</example>
    decimal Price,
    
    /// <summary>
    /// Official release date
    /// </summary>
    /// <example>2020-12-10</example>
    DateTime ReleaseDate,
    
    /// <summary>
    /// Publisher name (max 200 characters)
    /// </summary>
    /// <example>CD Projekt Red</example>
    string Publisher
) : IRequest<Result<Guid>>;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(3).WithMessage("Title must have at least 3 characters")
            .MaximumLength(200).WithMessage("Title must have at most 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must have at most 2000 characters");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .MaximumLength(100).WithMessage("Genre must have at most 100 characters");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required");

        RuleFor(x => x.Publisher)
            .NotEmpty().WithMessage("Publisher is required")
            .MaximumLength(200).WithMessage("Publisher must have at most 200 characters");
    }
}
