using MediatR;
using FluentValidation;

namespace TechChallenge.Application.Commands.Auth.DisableUser;

public record DisableUserCommand(string Email) : IRequest;

public class DisableUserCommandValidator : AbstractValidator<DisableUserCommand>
{
    public DisableUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
