using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Constants;
using UrlShortener.Entities;

namespace UrlShortener.Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {   
        }

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
}
