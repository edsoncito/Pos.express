namespace POS.Business.Rules;

public interface IStockRule
{
    bool AllowsSale(int currentStock, int quantity);
}
