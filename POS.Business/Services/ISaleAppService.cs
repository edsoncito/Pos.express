using POS.Entities;

namespace POS.Business.Services;

public interface ISaleAppService
{
    Task<ExpressSale> RegisterSaleAsync(int productId, int quantity, string customer);
}
