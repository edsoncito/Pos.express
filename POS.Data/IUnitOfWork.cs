using Microsoft.EntityFrameworkCore.Storage;
using POS.Data.Repositories;
using POS.Entities;

namespace POS.Data;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IRepository<ProductType> ProductTypes { get; }
    IRepository<Category> Categories { get; }
    IRepository<ProductCategory> ProductCategories { get; }
    IRepository<ErpProduct> ErpProducts { get; }
    IRepository<BarCode> BarCodes { get; }
    IRepository<ExpressSale> Sales { get; }

    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
