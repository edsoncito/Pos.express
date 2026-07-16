using POS.Data.Contexts;
using POS.Entities;

namespace POS.Data.Repositories;

public class SaleRepository : Repository<ExpressSale>, ISaleRepository
{
    public SaleRepository(PosExpressDbContext context) : base(context)
    {
    }
}
