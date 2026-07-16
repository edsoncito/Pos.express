using POS.Entities;

namespace POS.Business.Services;

public interface IProductAppService
{
    Task<ErpProduct> RegisterErpProductAsync(int productId, decimal cost);
}
