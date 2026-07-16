namespace POS.Business.Rules.Base;

public class BasePriceRule : IPriceRule
{
    public decimal CalculatePrice(decimal cost) => cost * 1.50m;
}
