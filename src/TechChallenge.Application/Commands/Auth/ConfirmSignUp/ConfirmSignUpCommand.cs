using MediatR;
using FluentValidation;
using TechChallenge.Application.Common.Models;

namespace TechChallenge.Application.Commands.Auth.ConfirmSignUp;

public record ConfirmSignUpCommand(string Email, string ConfirmationCode) : IRequest<Result>;

public class ConfirmSignUpCommandValidator : AbstractValidator<ConfirmSignUpCommand>
{
    public ConfirmSignUpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.ConfirmationCode)
            .NotEmpty().WithMessage("Confirmation code is required")
            .Length(6).WithMessage("Confirmation code must be 6 characters");
    }
}
