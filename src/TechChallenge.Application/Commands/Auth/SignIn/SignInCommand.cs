using MediatR;
using FluentValidation;
using TechChallenge.Application.Contracts.Auth.Responses;

namespace TechChallenge.Application.Commands.Auth.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<Token>;

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
