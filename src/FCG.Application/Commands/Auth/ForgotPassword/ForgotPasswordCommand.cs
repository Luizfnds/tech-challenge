using MediatR;
using FluentValidation;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Auth.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<Result>;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
