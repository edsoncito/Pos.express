using Moq;
using POS.Business.Generators;
using POS.Business.Services;
using POS.Data;
using POS.Data.Repositories;
using POS.Entities;
using Xunit;

namespace POS.Business.Tests.Services;

public class BarCodeAppServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProductRepository> _products = new();
    private readonly Mock<IRepository<BarCode>> _barCodes = new();
    private readonly Mock<IUniqueCodeGenerator> _codeGenerator = new();

    public BarCodeAppServiceTests()
    {
        _unitOfWork.Setup(u => u.Products).Returns(_products.Object);
        _unitOfWork.Setup(u => u.BarCodes).Returns(_barCodes.Object);
    }

    private BarCodeAppService CreateService() => new(_unitOfWork.Object, _codeGenerator.Object);

    [Fact]
    public async Task AssignBarCodeAsync_ProductoExiste_AsignaCodigoActivo()
    {
        var product = new Product { Id = 1, Name = "Producto A" };
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(product);
        _barCodes.Setup(b => b.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<BarCode, bool>>>()))
            .ReturnsAsync(Array.Empty<BarCode>());
        _codeGenerator.Setup(g => g.Generate()).Returns("260716000000000-BBBBBB");

        var service = CreateService();
        var barCode = await service.AssignBarCodeAsync(1);

        Assert.Equal("260716000000000-BBBBBB", barCode.UniqueCode);
        Assert.True(barCode.IsActive);
        Assert.Equal(1, barCode.ProductId);
        _barCodes.Verify(b => b.AddAsync(It.IsAny<BarCode>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AssignBarCodeAsync_ProductoNoExiste_LanzaExcepcion()
    {
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync((Product?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AssignBarCodeAsync(1));
    }
}
