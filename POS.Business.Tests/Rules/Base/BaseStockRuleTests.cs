using POS.Business.Rules.Base;
using Xunit;

namespace POS.Business.Tests.Rules.Base;

public class BaseStockRuleTests
{
    [Fact]
    public void AllowsSale_StockExactoParaLaCantidad_Permite()
    {
        var rule = new BaseStockRule();

        Assert.True(rule.AllowsSale(currentStock: 10, quantity: 10));
    }

    [Fact]
    public void AllowsSale_StockInsuficiente_Rechaza()
    {
        var rule = new BaseStockRule();

        Assert.False(rule.AllowsSale(currentStock: 5, quantity: 10));
    }
}
