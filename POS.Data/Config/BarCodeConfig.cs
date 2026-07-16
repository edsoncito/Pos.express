using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Entities;

namespace POS.Data.Config;

public class BarCodeConfig : IEntityTypeConfiguration<BarCode>
{
    public void Configure(EntityTypeBuilder<BarCode> builder)
    {
        builder.ToTable("BarCodes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UniqueCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.UniqueCode).IsUnique();

        builder.HasOne(x => x.Product)
            .WithMany(p => p.BarCodes)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
