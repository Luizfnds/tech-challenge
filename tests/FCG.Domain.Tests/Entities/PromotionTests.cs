using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.Domain.Tests.Entities;

public class PromotionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(101)]
    public void GivenInvalidDiscountPercentage_WhenCreating_ThenShouldThrowArgumentException(decimal invalidDiscount)
    {
        // Given (Dado)
        var act = () => Promotion.Create(Guid.NewGuid(), invalidDiscount, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Discount percentage must be between 0 and 100*");
    }

    [Fact]
    public void GivenStartDateAfterEndDate_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => Promotion.Create(Guid.NewGuid(), 25m, DateTime.UtcNow.AddDays(7), DateTime.UtcNow);

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Start date must be before end date*");
    }

    [Fact]
    public void GivenEndDateInThePast_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => Promotion.Create(Guid.NewGuid(), 25m, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow.AddDays(-1));

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("End date must be in the future*");
    }

    [Fact]
    public void GivenValidPromotion_WhenCalculatingDiscount_ThenShouldReturnCorrectPrice()
    {
        // Given (Dado)
        var promotion = Promotion.Create(Guid.NewGuid(), 25m, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddDays(7));

        // When (Quando)
        var discountedPrice = promotion.CalculateDiscountedPrice(100m);

        // Then (Então)
        discountedPrice.Should().Be(75m);
    }

    [Fact]
    public void GivenInactivePromotion_WhenCalculatingDiscount_ThenShouldReturnOriginalPrice()
    {
        // Given (Dado)
        var promotion = Promotion.Create(Guid.NewGuid(), 50m, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddDays(7));
        promotion.Deactivate();

        // When (Quando)
        var discountedPrice = promotion.CalculateDiscountedPrice(100m);

        // Then (Então)
        discountedPrice.Should().Be(100m);
    }
}
