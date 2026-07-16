namespace POS.Business.Services;

public interface ICategoryAppService
{
    Task AssignCategoryAsync(int productId, int categoryId);
}
