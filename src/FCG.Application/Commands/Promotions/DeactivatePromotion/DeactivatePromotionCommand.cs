using FCG.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace FCG.Application.Commands.Promotions.DeactivatePromotion;

public record DeactivatePromotionCommand(Guid PromotionId) : IRequest<Result>;

public class DeactivatePromotionCommandValidator : AbstractValidator<DeactivatePromotionCommand>
{
    public DeactivatePromotionCommandValidator()
    {
        RuleFor(x => x.PromotionId)
            .NotEmpty()
            .WithMessage("Promotion ID is required");
    }
}
