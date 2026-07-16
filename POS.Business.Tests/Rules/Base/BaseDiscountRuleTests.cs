using POS.Business.Rules.Base;
using Xunit;

namespace POS.Business.Tests.Rules.Base;

public class BaseDiscountRuleTests
{
    [Fact]
    public void CalculateDiscountRate_UnaCategoria_DevuelveTreintaPorCiento()
    {
        var rule = new BaseDiscountRule();

        var rate = rule.CalculateDiscountRate(1);

        Assert.Equal(0.30m, rate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(3)]
    public void CalculateDiscountRate_CeroODosOMasCategorias_DevuelveCero(int categoryCount)
    {
        var rule = new BaseDiscountRule();

        var rate = rule.CalculateDiscountRate(categoryCount);

        Assert.Equal(0m, rate);
    }
}
