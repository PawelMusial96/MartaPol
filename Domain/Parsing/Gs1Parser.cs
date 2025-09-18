using System.Text.RegularExpressions;

namespace MartaPol.Domain.Parsing;

// GS1 AIs: 310x (kg), 320x (lb)
public class Gs1Parser : IBarcodeWeightParser
{
    private static readonly Regex Ai310 = new(@"\(310(?<x>\d)\)(?<w>\d{6})", RegexOptions.Compiled);
    private static readonly Regex Ai320 = new(@"\(320(?<x>\d)\)(?<w>\d{6})", RegexOptions.Compiled);

    private static readonly Regex Ai310Plain = new(@"310(?<x>\d)(?<w>\d{6})", RegexOptions.Compiled);
    private static readonly Regex Ai320Plain = new(@"320(?<x>\d)(?<w>\d{6})", RegexOptions.Compiled);

    public WeightParseResult TryParseWeight(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return new(false, 0);

        var m = Ai310.Match(raw);
        if (!m.Success) m = Ai310Plain.Match(raw);
        if (m.Success)
        {
            int dec = int.Parse(m.Groups["x"].Value);
            var num = m.Groups["w"].Value;
            if (decimal.TryParse(num.Insert(num.Length - dec, "."), out var kg))
                return new(true, Math.Round(kg, 3), $"AI310{dec}");
        }

        m = Ai320.Match(raw);
        if (!m.Success) m = Ai320Plain.Match(raw);
        if (m.Success)
        {
            int dec = int.Parse(m.Groups["x"].Value);
            var num = m.Groups["w"].Value;
            if (decimal.TryParse(num.Insert(num.Length - dec, "."), out var lb))
            {
                var kg = lb * 0.45359237m;
                return new(true, Math.Round(kg, 3), $"AI320{dec}");
            }
        }

        return new(false, 0);
    }
}
