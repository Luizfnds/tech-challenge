using MediatR;
using FluentValidation;

namespace TechChallenge.Application.Commands.Auth.EnableUser;

public record EnableUserCommand(string Email) : IRequest;

public class EnableUserCommandValidator : AbstractValidator<EnableUserCommand>
{
    public EnableUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
