using Moq;
using POS.Business.Services;
using POS.Data;
using POS.Data.Repositories;
using POS.Entities;
using Xunit;

namespace POS.Business.Tests.Services;

public class ProductTypeAppServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IRepository<ProductType>> _productTypes = new();

    public ProductTypeAppServiceTests()
    {
        _unitOfWork.Setup(u => u.ProductTypes).Returns(_productTypes.Object);
    }

    [Fact]
    public async Task RegisterProductTypeAsync_CreaElTipoDeProducto()
    {
        var service = new ProductTypeAppService(_unitOfWork.Object);

        var productType = await service.RegisterProductTypeAsync("Medicamentos");

        Assert.Equal("Medicamentos", productType.Description);
        _productTypes.Verify(p => p.AddAsync(It.Is<ProductType>(x => x.Description == "Medicamentos")), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
