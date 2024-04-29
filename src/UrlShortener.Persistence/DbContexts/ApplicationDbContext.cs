using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Constants;

namespace UrlShortener.Persistence.DbContexts;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder
                .Property(c => c.Code)
                .HasMaxLength(ShortLinkSettings.Length);

            builder
                .HasIndex(c => c.Code)
                .IsUnique();
        });
    }
}
