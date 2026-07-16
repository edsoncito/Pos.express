namespace POS.Business.Rules.Base;

public class BaseStockRule : IStockRule
{
    public bool AllowsSale(int currentStock, int quantity) => currentStock >= quantity;
}
