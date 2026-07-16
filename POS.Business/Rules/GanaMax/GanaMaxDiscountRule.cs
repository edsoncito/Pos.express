namespace POS.Business.Rules.GanaMax;

public class GanaMaxDiscountRule : IDiscountRule
{
    public decimal CalculateDiscountRate(int associatedCategoryCount)
    {
        return associatedCategoryCount == 1 ? 0.10m : 0m;
    }
}
