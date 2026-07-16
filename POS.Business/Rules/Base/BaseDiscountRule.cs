namespace POS.Business.Rules.Base;

public class BaseDiscountRule : IDiscountRule
{
    public decimal CalculateDiscountRate(int associatedCategoryCount)
    {
        return associatedCategoryCount == 1 ? 0.30m : 0m;
    }
}
