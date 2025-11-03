using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Games.ActivateGame;

public record ActivateGameCommand(Guid Id) : IRequest<Result>;

public class ActivateGameCommandValidator : AbstractValidator<ActivateGameCommand>
{
    public ActivateGameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Game ID is required");
    }
}
