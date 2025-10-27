using MediatR;
using FluentValidation;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Commands.CreateUser;

// TODO: Trocar para record
public class CreateUserCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must have at least 3 characters")
            .MaximumLength(100).WithMessage("Name must have at most 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(150).WithMessage("Email must have at most 150 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters");
    }
}
