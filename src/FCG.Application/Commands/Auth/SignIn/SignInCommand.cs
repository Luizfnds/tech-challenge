using MediatR;
using FluentValidation;
using FCG.Application.Contracts.Auth.Responses;
using FCG.Application.Common.Models;

namespace FCG.Application.Commands.Auth.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<Result<Token>>;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
