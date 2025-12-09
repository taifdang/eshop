using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VariantOptionConfiguration : IEntityTypeConfiguration<VariantOption>
{
    public void Configure(EntityTypeBuilder<VariantOption> builder)
    {
        builder.ToTable(nameof(VariantOption));

        builder.HasKey(x => new { x.VariantId, x.OptionValueId });

        builder.HasIndex(ci => ci.OptionValueId);
        builder.HasIndex(ci => ci.VariantId);

        builder.HasOne(x => x.OptionValue)
            .WithMany()
            .HasForeignKey(ci => ci.OptionValueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
