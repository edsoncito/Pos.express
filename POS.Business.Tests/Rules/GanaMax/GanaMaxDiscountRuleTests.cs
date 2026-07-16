using POS.Business.Rules.GanaMax;
using Xunit;

namespace POS.Business.Tests.Rules.GanaMax;

public class GanaMaxDiscountRuleTests
{
    [Fact]
    public void CalculateDiscountRate_UnaCategoria_DevuelveDiezPorCiento()
    {
        var rule = new GanaMaxDiscountRule();

        var rate = rule.CalculateDiscountRate(1);

        Assert.Equal(0.10m, rate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(3)]
    public void CalculateDiscountRate_CeroODosOMasCategorias_DevuelveCero(int categoryCount)
    {
        var rule = new GanaMaxDiscountRule();

        var rate = rule.CalculateDiscountRate(categoryCount);

        Assert.Equal(0m, rate);
    }
}
