namespace POS.Entities;

public class ExpressSale
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Customer { get; set; } = string.Empty;

    // Snapshot of the product name at the time of sale (the "Producto" column in the original diagram).
    public string ProductName { get; set; } = string.Empty;

    // BR2: snapshot of the product's UniqueCode at the time of sale.
    public string ProductUniqueCode { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
