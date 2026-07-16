using Moq;
using POS.Business.Generators;
using POS.Business.Rules;
using POS.Business.Services;
using POS.Data;
using POS.Data.Repositories;
using POS.Entities;
using Xunit;

namespace POS.Business.Tests.Services;

public class ProductAppServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProductRepository> _products = new();
    private readonly Mock<IRepository<ErpProduct>> _erpProducts = new();
    private readonly Mock<IUniqueCodeGenerator> _codeGenerator = new();
    private readonly Mock<IPriceRule> _priceRule = new();

    public ProductAppServiceTests()
    {
        _unitOfWork.Setup(u => u.Products).Returns(_products.Object);
        _unitOfWork.Setup(u => u.ErpProducts).Returns(_erpProducts.Object);
    }

    private ProductAppService CreateService() =>
        new(_unitOfWork.Object, _codeGenerator.Object, _priceRule.Object);

    [Fact]
    public async Task RegisterErpProductAsync_ProductoExiste_CalculaPrecioYRegistraErp()
    {
        var product = new Product { Id = 1, Name = "Producto A" };
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(product);
        _erpProducts.Setup(e => e.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ErpProduct, bool>>>()))
            .ReturnsAsync(Array.Empty<ErpProduct>());
        _codeGenerator.Setup(g => g.Generate()).Returns("260716000000000-AAAAAA");
        _priceRule.Setup(p => p.CalculatePrice(100m)).Returns(150m);

        var service = CreateService();
        var erpProduct = await service.RegisterErpProductAsync(1, 100m);

        Assert.Equal("260716000000000-AAAAAA", erpProduct.UniqueCode);
        Assert.Equal(100m, erpProduct.Cost);
        Assert.Equal(0, erpProduct.Stock);
        Assert.Equal(150m, product.Price);
        _products.Verify(p => p.Update(product), Times.Once);
        _erpProducts.Verify(e => e.AddAsync(It.IsAny<ErpProduct>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RegisterErpProductAsync_HayColisionDeCodigo_Reintenta()
    {
        var product = new Product { Id = 1, Name = "Producto A" };
        var existing = new ErpProduct { ProductId = 99, UniqueCode = "CODE-REPETIDO" };
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(product);
        _erpProducts.SetupSequence(e => e.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ErpProduct, bool>>>()))
            .ReturnsAsync(new[] { existing })
            .ReturnsAsync(Array.Empty<ErpProduct>());
        _codeGenerator.SetupSequence(g => g.Generate())
            .Returns("CODE-REPETIDO")
            .Returns("CODE-NUEVO");
        _priceRule.Setup(p => p.CalculatePrice(It.IsAny<decimal>())).Returns(150m);

        var service = CreateService();
        var erpProduct = await service.RegisterErpProductAsync(1, 100m);

        Assert.Equal("CODE-NUEVO", erpProduct.UniqueCode);
        _codeGenerator.Verify(g => g.Generate(), Times.Exactly(2));
    }

    [Fact]
    public async Task RegisterErpProductAsync_ProductoNoExiste_LanzaExcepcion()
    {
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync((Product?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterErpProductAsync(1, 100m));
    }
}
