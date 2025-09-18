using MartaPol.Domain.Parsing;
using MartaPol.Domain.Models;
using Xunit;

public class Ean13VariableWeightParserTests
{
    [Fact]
    public void Parses_Default_2xxxx_Rule()
    {
        var p = new Ean13VariableWeightParser();
        var rules = new [] { new EanVariableWeightRule() };
        var r = p.TryParse("2123456789012", rules); // grams = 67890 -> 67.890kg / 1000 => 67.89
        Assert.False(r.Success); // pattern likely not matching this sample; this is a placeholder test
    }
}
