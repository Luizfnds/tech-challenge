using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Games.DeactivateGame;

public record DeactivateGameCommand(Guid Id) : IRequest<Result>;

public class DeactivateGameCommandValidator : AbstractValidator<DeactivateGameCommand>
{
    public DeactivateGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Game ID is required");
    }
}
