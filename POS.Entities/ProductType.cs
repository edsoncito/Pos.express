namespace POS.Entities;

public class ProductType
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
