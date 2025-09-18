using MartaPol.Domain.Parsing;
using Xunit;

public class Gs1ParserTests
{
    [Fact]
    public void Parses_AI310_Kg()
    {
        var p = new Gs1Parser();
        var r = p.TryParseWeight("(3103)001234");
        Assert.True(r.Success);
        Assert.Equal(1.234m, r.WeightKg);
    }

    [Fact]
    public void Parses_AI320_Lb_To_Kg()
    {
        var p = new Gs1Parser();
        var r = p.TryParseWeight("3203001234");
        Assert.True(r.Success);
        Assert.True(r.WeightKg > 0);
    }
}
