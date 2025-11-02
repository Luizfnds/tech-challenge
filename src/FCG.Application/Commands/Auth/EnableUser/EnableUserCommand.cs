using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Auth.EnableUser;

public record EnableUserCommand(string Email) : IRequest<Result>;

public class EnableUserCommandValidator : AbstractValidator<EnableUserCommand>
{
    public EnableUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
