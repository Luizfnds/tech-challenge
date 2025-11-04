using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations;

public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
{
    public void Configure(EntityTypeBuilder<UserGame> builder)
    {
        builder.ToTable("UserGames");

        builder.HasKey(ug => ug.Id);

        builder.Property(ug => ug.UserId)
            .IsRequired();

        builder.Property(ug => ug.GameId)
            .IsRequired();

        builder.Property(ug => ug.PurchaseDate)
            .IsRequired();

        builder.Property(ug => ug.PurchasePrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(ug => ug.User)
            .WithMany(u => u.UserGames)
            .HasForeignKey(ug => ug.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ug => ug.Game)
            .WithMany(g => g.UserGames)
            .HasForeignKey(ug => ug.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ug => new { ug.UserId, ug.GameId })
            .IsUnique()
            .HasDatabaseName("IX_UserGames_UserId_GameId");

        builder.HasIndex(ug => ug.UserId)
            .HasDatabaseName("IX_UserGames_UserId");

        builder.HasIndex(ug => ug.GameId)
            .HasDatabaseName("IX_UserGames_GameId");

        builder.HasIndex(ug => ug.PurchaseDate)
            .HasDatabaseName("IX_UserGames_PurchaseDate");
    }
}
