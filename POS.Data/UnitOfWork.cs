using Microsoft.EntityFrameworkCore.Storage;
using POS.Data.Contexts;
using POS.Data.Repositories;
using POS.Entities;

namespace POS.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly PosExpressDbContext _context;

    public UnitOfWork(PosExpressDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        ProductTypes = new Repository<ProductType>(context);
        Categories = new Repository<Category>(context);
        ProductCategories = new Repository<ProductCategory>(context);
        ErpProducts = new Repository<ErpProduct>(context);
        BarCodes = new Repository<BarCode>(context);
        Sales = new Repository<ExpressSale>(context);
    }

    public IProductRepository Products { get; }
    public IRepository<ProductType> ProductTypes { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<ProductCategory> ProductCategories { get; }
    public IRepository<ErpProduct> ErpProducts { get; }
    public IRepository<BarCode> BarCodes { get; }
    public IRepository<ExpressSale> Sales { get; }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();

    public void Dispose() => _context.Dispose();
}
