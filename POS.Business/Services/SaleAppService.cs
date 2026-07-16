using POS.Data;
using POS.Business.Rules;
using POS.Entities;

namespace POS.Business.Services;

public class SaleAppService : ISaleAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscountRule _discountRule;
    private readonly IStockRule _stockRule;

    public SaleAppService(IUnitOfWork unitOfWork, IDiscountRule discountRule, IStockRule stockRule)
    {
        _unitOfWork = unitOfWork;
        _discountRule = discountRule;
        _stockRule = stockRule;
    }

    public async Task<ExpressSale> RegisterSaleAsync(int productId, int quantity, string customer)
    {
        var product = await _unitOfWork.Products.GetWithDetailsAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found.");

        if (product.ErpProduct == null)
            throw new InvalidOperationException("Product has not been registered in ERP.");

        if (!_stockRule.AllowsSale(product.ErpProduct.Stock, quantity))
            throw new InvalidOperationException($"Insufficient stock. Available: {product.ErpProduct.Stock}, Requested: {quantity}");

        var categoriesCount = product.ProductCategories?.Count ?? 0;
        var discountRate = _discountRule.CalculateDiscountRate(categoriesCount);

        var productUniqueCode = product.ErpProduct.UniqueCode;

        var subtotal = product.Price * quantity;
        var discount = subtotal * discountRate;
        var total = subtotal - discount;

        var sale = new ExpressSale
        {
            Date = DateTime.UtcNow,
            Customer = customer,
            ProductName = product.Name,
            ProductUniqueCode = productUniqueCode,
            Quantity = quantity,
            Price = product.Price,
            Discount = discount,
            Total = total,
            ProductId = productId
        };

        product.ErpProduct.Stock -= quantity;
        _unitOfWork.ErpProducts.Update(product.ErpProduct);

        await _unitOfWork.Sales.AddAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        return sale;
    }
}
