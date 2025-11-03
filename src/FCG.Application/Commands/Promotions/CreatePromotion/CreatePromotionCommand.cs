using FCG.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace FCG.Application.Commands.Promotions.CreatePromotion;

public record CreatePromotionCommand(
    Guid GameId,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate) : IRequest<Result<Guid>>;

public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("Game ID is required");

        RuleFor(x => x.DiscountPercentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Discount percentage must be between 0 and 100");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
    }
}
