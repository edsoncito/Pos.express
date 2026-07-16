namespace POS.Business.Rules;

public interface IPriceRule
{
    decimal CalculatePrice(decimal cost);
}
