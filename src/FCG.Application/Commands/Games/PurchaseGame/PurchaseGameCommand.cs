using FCG.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace FCG.Application.Commands.Games.PurchaseGame;

public record PurchaseGameCommand(Guid UserId, Guid GameId) : IRequest<Result<Guid>>;

public class PurchaseGameCommandValidator : AbstractValidator<PurchaseGameCommand>
{
    public PurchaseGameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("Game ID is required");
    }
}
