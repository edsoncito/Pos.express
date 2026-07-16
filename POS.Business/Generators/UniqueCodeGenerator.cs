namespace POS.Business.Generators;

public class UniqueCodeGenerator : IUniqueCodeGenerator
{
    public string Generate()
    {
        var timestamp = DateTime.UtcNow.ToString("yyMMddHHmmssfff");
        var guid = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        return $"{timestamp}-{guid}";
    }
}
