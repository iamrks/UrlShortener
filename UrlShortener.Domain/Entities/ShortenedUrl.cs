using UrlShortener.Domain.Primitives;

namespace UrlShortener.Domain.Entities;

public sealed class ShortenedUrl : IAuditableEntity
{
    public Guid Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
}
