using Moq;
using POS.Business.Rules;
using POS.Business.Services;
using POS.Data;
using POS.Data.Repositories;
using POS.Entities;
using Xunit;

namespace POS.Business.Tests.Services;

public class SaleAppServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProductRepository> _products = new();
    private readonly Mock<IRepository<ErpProduct>> _erpProducts = new();
    private readonly Mock<ISaleRepository> _sales = new();
    private readonly Mock<IDiscountRule> _discountRule = new();
    private readonly Mock<IStockRule> _stockRule = new();

    public SaleAppServiceTests()
    {
        _unitOfWork.Setup(u => u.Products).Returns(_products.Object);
        _unitOfWork.Setup(u => u.ErpProducts).Returns(_erpProducts.Object);
        _unitOfWork.Setup(u => u.Sales).Returns(_sales.Object);
    }

    private SaleAppService CreateService() => new(_unitOfWork.Object, _discountRule.Object, _stockRule.Object);

    private static Product CreateProductConErp(int stock, int categoryCount)
    {
        var product = new Product
        {
            Id = 1,
            Name = "Producto A",
            Price = 150m,
            ErpProduct = new ErpProduct { ProductId = 1, UniqueCode = "260716000000000-AAAAAA", Stock = stock }
        };

        for (var i = 0; i < categoryCount; i++)
            product.ProductCategories.Add(new ProductCategory { ProductId = 1, CategoryId = i + 1 });

        return product;
    }

    [Fact]
    public async Task RegisterSaleAsync_ConUnaCategoria_AplicaDescuentoYDescuentaStock()
    {
        var product = CreateProductConErp(stock: 50, categoryCount: 1);
        _products.Setup(p => p.GetWithDetailsAsync(1)).ReturnsAsync(product);
        _stockRule.Setup(s => s.AllowsSale(50, 5)).Returns(true);
        _discountRule.Setup(d => d.CalculateDiscountRate(1)).Returns(0.30m);

        var service = CreateService();
        var sale = await service.RegisterSaleAsync(1, 5, "Cliente1");

        Assert.Equal("Producto A", sale.ProductName);
        Assert.Equal("260716000000000-AAAAAA", sale.ProductUniqueCode);
        Assert.Equal(150m, sale.Price);
        Assert.Equal(225m, sale.Discount);
        Assert.Equal(525m, sale.Total);
        Assert.Equal(45, product.ErpProduct!.Stock);
        _erpProducts.Verify(e => e.Update(product.ErpProduct), Times.Once);
        _sales.Verify(s => s.AddAsync(It.IsAny<ExpressSale>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterSaleAsync_ConDosCategorias_NoAplicaDescuento()
    {
        var product = CreateProductConErp(stock: 50, categoryCount: 2);
        _products.Setup(p => p.GetWithDetailsAsync(1)).ReturnsAsync(product);
        _stockRule.Setup(s => s.AllowsSale(50, 5)).Returns(true);
        _discountRule.Setup(d => d.CalculateDiscountRate(2)).Returns(0m);

        var service = CreateService();
        var sale = await service.RegisterSaleAsync(1, 5, "Cliente2");

        Assert.Equal(0m, sale.Discount);
        Assert.Equal(750m, sale.Total);
    }

    [Fact]
    public async Task RegisterSaleAsync_StockRuleRechazaLaVenta_LanzaExcepcion()
    {
        var product = CreateProductConErp(stock: 5, categoryCount: 0);
        _products.Setup(p => p.GetWithDetailsAsync(1)).ReturnsAsync(product);
        _stockRule.Setup(s => s.AllowsSale(5, 10)).Returns(false);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterSaleAsync(1, 10, "Cliente3"));
        _sales.Verify(s => s.AddAsync(It.IsAny<ExpressSale>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task RegisterSaleAsync_ProductoNoExiste_LanzaExcepcion()
    {
        _products.Setup(p => p.GetWithDetailsAsync(1)).ReturnsAsync((Product?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterSaleAsync(1, 1, "Cliente4"));
    }

    [Fact]
    public async Task RegisterSaleAsync_ProductoSinRegistroErp_LanzaExcepcion()
    {
        var product = new Product { Id = 1, Name = "Producto sin ERP" };
        _products.Setup(p => p.GetWithDetailsAsync(1)).ReturnsAsync(product);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterSaleAsync(1, 1, "Cliente5"));
    }
}
