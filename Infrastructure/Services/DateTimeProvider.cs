using MartaPol.Domain.Abstractions;

namespace MartaPol.Infrastructure.Services;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime NowLocal => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
