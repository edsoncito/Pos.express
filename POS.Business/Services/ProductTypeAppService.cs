using POS.Data;
using POS.Entities;

namespace POS.Business.Services;

public class ProductTypeAppService : IProductTypeAppService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductTypeAppService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductType> RegisterProductTypeAsync(string description)
    {
        var productType = new ProductType { Description = description };

        await _unitOfWork.ProductTypes.AddAsync(productType);
        await _unitOfWork.SaveChangesAsync();

        return productType;
    }
}
