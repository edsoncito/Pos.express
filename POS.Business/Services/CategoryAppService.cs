using POS.Data;
using POS.Entities;

namespace POS.Business.Services;

public class CategoryAppService : ICategoryAppService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryAppService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AssignCategoryAsync(int productId, int categoryId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found.");

        var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
        if (category == null)
            throw new InvalidOperationException($"Category with ID {categoryId} not found.");

        var productCategory = new ProductCategory
        {
            ProductId = productId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ProductCategories.AddAsync(productCategory);
        await _unitOfWork.SaveChangesAsync();
    }
}
