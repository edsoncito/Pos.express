namespace POS.Entities;

public class ErpProduct
{
    // Shared PK with Product (1-1 relationship: extends Product with ERP inventory data).
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public decimal Cost { get; set; }
    public string UniqueCode { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; }
    public int Stock { get; set; }
}
