namespace MartaPol.Domain.Parsing;

public record WeightParseResult(bool Success, decimal WeightKg, string? Info = null);
