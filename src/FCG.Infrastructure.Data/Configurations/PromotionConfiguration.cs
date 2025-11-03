using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Data.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.GameId)
            .IsRequired();

        builder.Property(p => p.DiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(p => p.Game)
            .WithOne(g => g.Promotion)
            .HasForeignKey<Promotion>(p => p.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.GameId)
            .IsUnique()
            .HasDatabaseName("IX_Promotions_GameId");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Promotions_IsActive");

        builder.HasIndex(p => new { p.StartDate, p.EndDate })
            .HasDatabaseName("IX_Promotions_DateRange");
    }
}
