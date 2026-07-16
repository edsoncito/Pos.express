namespace POS.Business.Rules.GanaMax;

public class GanaMaxStockRule : IStockRule
{
    public bool AllowsSale(int currentStock, int quantity)
    {
        return currentStock >= quantity && (currentStock - quantity) > 10;
    }
}
