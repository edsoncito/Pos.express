namespace POS.Entities;

public class BarCode
{
    public int Id { get; set; }
    public string UniqueCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
