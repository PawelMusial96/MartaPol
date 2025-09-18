namespace MartaPol.Domain.Models;

public class EanVariableWeightRule
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "Domyślna 2xxxxx (gramy)";
    public string Pattern { get; set; } = "^2\\d{6}(?<grams>\\d{5})\\d$";
    public int DivideBy { get; set; } = 1000;
    public bool Enabled { get; set; } = true;
}
