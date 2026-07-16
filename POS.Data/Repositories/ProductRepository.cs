using Microsoft.EntityFrameworkCore;
using POS.Data.Contexts;
using POS.Entities;

namespace POS.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(PosExpressDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetWithDetailsAsync(int id) =>
        await Context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ErpProduct)
            .Include(p => p.BarCodes)
            .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
}
