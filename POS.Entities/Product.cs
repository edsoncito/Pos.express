namespace POS.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Notes { get; set; }

    public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; } = null!;

    public ErpProduct? ErpProduct { get; set; }
    public ICollection<BarCode> BarCodes { get; set; } = new List<BarCode>();
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public ICollection<ExpressSale> Sales { get; set; } = new List<ExpressSale>();
}
