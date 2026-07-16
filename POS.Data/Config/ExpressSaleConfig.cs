using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Entities;

namespace POS.Data.Config;

public class ExpressSaleConfig : IEntityTypeConfiguration<ExpressSale>
{
    public void Configure(EntityTypeBuilder<ExpressSale> builder)
    {
        builder.ToTable("ExpressSales");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Customer).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.ProductUniqueCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Total).HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Product)
            .WithMany(p => p.Sales)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
