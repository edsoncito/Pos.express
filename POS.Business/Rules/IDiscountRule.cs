namespace POS.Business.Rules;

public interface IDiscountRule
{
    decimal CalculateDiscountRate(int associatedCategoryCount);
}
