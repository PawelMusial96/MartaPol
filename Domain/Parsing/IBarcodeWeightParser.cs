namespace MartaPol.Domain.Parsing;

public interface IBarcodeWeightParser
{
    WeightParseResult TryParseWeight(string raw);
}
