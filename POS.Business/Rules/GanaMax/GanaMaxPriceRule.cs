namespace POS.Business.Rules.GanaMax;

public class GanaMaxPriceRule : IPriceRule
{
    public decimal CalculatePrice(decimal cost) => cost * 1.80m;
}
