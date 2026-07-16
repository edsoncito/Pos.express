using Microsoft.EntityFrameworkCore;
using POS.Entities;

namespace POS.Data.Contexts;

public class PosExpressDbContext : DbContext
{
    public PosExpressDbContext(DbContextOptions<PosExpressDbContext> options) : base(options)
    {
    }

    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ErpProduct> ErpProducts => Set<ErpProduct>();
    public DbSet<BarCode> BarCodes => Set<BarCode>();
    public DbSet<ExpressSale> ExpressSales => Set<ExpressSale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PosExpressDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
