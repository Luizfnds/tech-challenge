using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.Domain.Tests.Entities;

public class UserGameTests
{
    [Fact]
    public void GivenEmptyUserId_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => UserGame.Create(Guid.Empty, Guid.NewGuid(), 59.99m);

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("User ID cannot be empty*");
    }

    [Fact]
    public void GivenEmptyGameId_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => UserGame.Create(Guid.NewGuid(), Guid.Empty, 59.99m);

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Game ID cannot be empty*");
    }

    [Fact]
    public void GivenNegativePurchasePrice_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => UserGame.Create(Guid.NewGuid(), Guid.NewGuid(), -10m);

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Purchase price cannot be negative*");
    }
}
