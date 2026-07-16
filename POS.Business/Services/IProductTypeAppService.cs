using POS.Entities;

namespace POS.Business.Services;

public interface IProductTypeAppService
{
    Task<ProductType> RegisterProductTypeAsync(string description);
}
