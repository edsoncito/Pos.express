using POS.Business.Rules.GanaMax;
using Xunit;

namespace POS.Business.Tests.Rules.GanaMax;

public class GanaMaxStockRuleTests
{
    [Fact]
    public void AllowsSale_StockInsuficienteParaLaCantidad_Rechaza()
    {
        var rule = new GanaMaxStockRule();

        Assert.False(rule.AllowsSale(currentStock: 5, quantity: 10));
    }

    [Fact]
    public void AllowsSale_AlcanzaLaCantidadPeroElRemanenteNoSuperaDiez_Rechaza()
    {
        var rule = new GanaMaxStockRule();

        // 15 - 5 = 10, que no es > 10, así que debe rechazarse aunque el stock alcance.
        Assert.False(rule.AllowsSale(currentStock: 15, quantity: 5));
    }

    [Fact]
    public void AllowsSale_RemanenteSuperaDiez_Permite()
    {
        var rule = new GanaMaxStockRule();

        // 16 - 5 = 11, que sí es > 10.
        Assert.True(rule.AllowsSale(currentStock: 16, quantity: 5));
    }
}
