using System.Text.RegularExpressions;
using MartaPol.Domain.Models;

namespace MartaPol.Domain.Parsing;

public class Ean13VariableWeightParser
{
    public WeightParseResult TryParse(string ean13, IEnumerable<EanVariableWeightRule> rules)
    {
        if (string.IsNullOrWhiteSpace(ean13) || ean13.Length != 13 || ean13[0] != '2')
            return new(false, 0);

        foreach (var r in rules.Where(r => r.Enabled))
        {
            var rx = new Regex(r.Pattern);
            var m = rx.Match(ean13);
            if (m.Success)
            {
                if (m.Groups["grams"].Success && int.TryParse(m.Groups["grams"].Value, out var g))
                {
                    var kg = (decimal)g / r.DivideBy;
                    return new(true, Math.Round(kg, 3), r.Name);
                }
            }
        }
        return new(false, 0);
    }
}
