using POS.Business.Rules.Base;
using Xunit;

namespace POS.Business.Tests.Rules.Base;

public class BasePriceRuleTests
{
    [Fact]
    public void CalculatePrice_AplicaMargenDelCincuentaPorCiento()
    {
        var rule = new BasePriceRule();

        var price = rule.CalculatePrice(100m);

        Assert.Equal(150m, price);
    }
}
