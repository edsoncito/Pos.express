using Moq;
using POS.Business.Services;
using POS.Data;
using POS.Data.Repositories;
using POS.Entities;
using Xunit;

namespace POS.Business.Tests.Services;

public class CategoryAppServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProductRepository> _products = new();
    private readonly Mock<IRepository<Category>> _categories = new();
    private readonly Mock<IRepository<ProductCategory>> _productCategories = new();

    public CategoryAppServiceTests()
    {
        _unitOfWork.Setup(u => u.Products).Returns(_products.Object);
        _unitOfWork.Setup(u => u.Categories).Returns(_categories.Object);
        _unitOfWork.Setup(u => u.ProductCategories).Returns(_productCategories.Object);
    }

    private CategoryAppService CreateService() => new(_unitOfWork.Object);

    [Fact]
    public async Task AssignCategoryAsync_ProductoYCategoriaExisten_CreaLaAsociacion()
    {
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(new Product { Id = 1 });
        _categories.Setup(c => c.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(new Category { Id = 2 });

        var service = CreateService();
        await service.AssignCategoryAsync(1, 2);

        _productCategories.Verify(pc => pc.AddAsync(It.Is<ProductCategory>(x => x.ProductId == 1 && x.CategoryId == 2)), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AssignCategoryAsync_ProductoNoExiste_LanzaExcepcion()
    {
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync((Product?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AssignCategoryAsync(1, 2));
    }

    [Fact]
    public async Task AssignCategoryAsync_CategoriaNoExiste_LanzaExcepcion()
    {
        _products.Setup(p => p.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync(new Product { Id = 1 });
        _categories.Setup(c => c.GetByIdAsync(It.IsAny<object[]>())).ReturnsAsync((Category?)null);

        var service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.AssignCategoryAsync(1, 2));
    }
}
