using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FCG.Domain.Entities;

namespace FCG.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(g => g.Genre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(g => g.ReleaseDate)
            .IsRequired();

        builder.Property(g => g.Publisher)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.Property(g => g.UpdatedAt)
            .IsRequired();

        builder.HasIndex(g => g.Title);
        builder.HasIndex(g => g.Genre);
        builder.HasIndex(g => g.IsActive);
    }
}
