using System.Text.RegularExpressions;
using POS.Business.Generators;
using Xunit;

namespace POS.Business.Tests.Generators;

public class UniqueCodeGeneratorTests
{
    [Fact]
    public void Generate_DevuelveFormatoTimestampMasSufijoHex()
    {
        var generator = new UniqueCodeGenerator();

        var code = generator.Generate();

        Assert.Matches(new Regex(@"^\d{15}-[0-9A-F]{6}$"), code);
    }

    [Fact]
    public void Generate_LlamadasSucesivasDevuelvenValoresDistintos()
    {
        var generator = new UniqueCodeGenerator();

        var first = generator.Generate();
        var second = generator.Generate();

        Assert.NotEqual(first, second);
    }
}
