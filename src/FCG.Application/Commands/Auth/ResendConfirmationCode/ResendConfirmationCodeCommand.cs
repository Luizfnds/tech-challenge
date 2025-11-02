using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Auth.ResendConfirmationCode;

public record ResendConfirmationCodeCommand(string Email) : IRequest<Result>;

public class ResendConfirmationCodeCommandValidator : AbstractValidator<ResendConfirmationCodeCommand>
{
    public ResendConfirmationCodeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
