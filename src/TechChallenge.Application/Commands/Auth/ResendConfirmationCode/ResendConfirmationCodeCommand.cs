using MediatR;
using FluentValidation;

namespace TechChallenge.Application.Commands.Auth.ResendConfirmationCode;

public record ResendConfirmationCodeCommand(string Email) : IRequest;

public class ResendConfirmationCodeCommandValidator : AbstractValidator<ResendConfirmationCodeCommand>
{
    public ResendConfirmationCodeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
