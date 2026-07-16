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

    public async Task<ErpProduct> RegisterErpProductAsync(int productId, decimal cost)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found.");

        var uniqueCode = _codeGenerator.Generate();

        while ((await _unitOfWork.ErpProducts.FindAsync(e => e.UniqueCode == uniqueCode)).Any())
        {
            uniqueCode = _codeGenerator.Generate();
        }

        var calculatedPrice = _priceRule.CalculatePrice(cost);

        product.Price = calculatedPrice;
        _unitOfWork.Products.Update(product);

        var erpProduct = new ErpProduct
        {
            ProductId = productId,
            UniqueCode = uniqueCode,
            Cost = cost,
            Stock = 0,
            RegisteredAt = DateTime.UtcNow
        };

        await _unitOfWork.ErpProducts.AddAsync(erpProduct);
        await _unitOfWork.SaveChangesAsync();

        return erpProduct;
    }
}
