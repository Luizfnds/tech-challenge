using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.Domain.Tests.Entities;

public class GameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenEmptyTitle_WhenCreating_ThenShouldThrowArgumentException(string invalidTitle)
    {
        // Given (Dado)
        var act = () => Game.Create(invalidTitle, "Description", "Genre", 59.99m, DateTime.UtcNow, "Publisher");

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Title cannot be empty*");
    }

    [Fact]
    public void GivenNegativePrice_WhenCreating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var act = () => Game.Create("Title", "Description", "Genre", -10m, DateTime.UtcNow, "Publisher");

        // When & Then (Quando & Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Price cannot be negative*");
    }

    [Fact]
    public void GivenNegativePrice_WhenUpdating_ThenShouldThrowArgumentException()
    {
        // Given (Dado)
        var game = Game.Create("Title", "Description", "Genre", 49.99m, DateTime.UtcNow, "Publisher");

        // When (Quando)
        var act = () => game.Update("Title", "Desc", "Genre", -10m, DateTime.UtcNow, "Publisher");

        // Then (Então)
        act.Should().Throw<ArgumentException>()
            .WithMessage("Price cannot be negative*");
    }
}
