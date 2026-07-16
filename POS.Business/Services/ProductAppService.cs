using POS.Data;
using POS.Business.Generators;
using POS.Business.Rules;
using POS.Entities;

namespace POS.Business.Services;

public class ProductAppService : IProductAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueCodeGenerator _codeGenerator;
    private readonly IPriceRule _priceRule;

    public ProductAppService(IUnitOfWork unitOfWork, IUniqueCodeGenerator codeGenerator, IPriceRule priceRule)
    {
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
        _priceRule = priceRule;
    }

    public async Task<ErpProduct> RegisterErpProductAsync(string name, int productTypeId, decimal cost, int stock)
    {
        var productType = await _unitOfWork.ProductTypes.GetByIdAsync(productTypeId);
        if (productType == null)
            throw new InvalidOperationException($"ProductType with ID {productTypeId} not found.");

        var uniqueCode = _codeGenerator.Generate();

        while ((await _unitOfWork.ErpProducts.FindAsync(e => e.UniqueCode == uniqueCode)).Any())
        {
            uniqueCode = _codeGenerator.Generate();
        }

        var product = new Product
        {
            Name = name,
            ProductTypeId = productTypeId,
            Price = _priceRule.CalculatePrice(cost),
            IsActive = true
        };

        var erpProduct = new ErpProduct
        {
            Product = product,
            UniqueCode = uniqueCode,
            Cost = cost,
            Stock = stock,
            RegisteredAt = DateTime.UtcNow
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.ErpProducts.AddAsync(erpProduct);
        await _unitOfWork.SaveChangesAsync();

        return erpProduct;
    }
}
