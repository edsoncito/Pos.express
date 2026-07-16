using POS.Business.Rules.GanaMax;
using Xunit;

namespace POS.Business.Tests.Rules.GanaMax;

public class GanaMaxPriceRuleTests
{
    [Fact]
    public void CalculatePrice_AplicaMargenDelOchentaPorCiento()
    {
        var rule = new GanaMaxPriceRule();

        var price = rule.CalculatePrice(100m);

        Assert.Equal(180m, price);
    }
}
