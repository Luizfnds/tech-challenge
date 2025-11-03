using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace FCG.Application.Commands.Games.CreateGame;

public record CreateGameCommand(
    string Title,
    string Description,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
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
