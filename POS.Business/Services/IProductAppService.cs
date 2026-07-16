using POS.Entities;

namespace POS.Business.Services;

public interface IProductAppService
{
    Task<ErpProduct> RegisterErpProductAsync(string name, int productTypeId, decimal cost, int stock);
}
