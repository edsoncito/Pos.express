namespace POS.Entities;

public class Category
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();

    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
