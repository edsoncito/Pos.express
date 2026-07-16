using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POS.Entities;

namespace POS.Data.Config;

public class ErpProductConfig : IEntityTypeConfiguration<ErpProduct>
{
    public void Configure(EntityTypeBuilder<ErpProduct> builder)
    {
        builder.ToTable("ErpProducts");
        builder.HasKey(x => x.ProductId);
        builder.Property(x => x.Cost).HasColumnType("decimal(18,2)");
        builder.Property(x => x.UniqueCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.UniqueCode).IsUnique();

        builder.HasOne(x => x.Product)
            .WithOne(p => p.ErpProduct)
            .HasForeignKey<ErpProduct>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
