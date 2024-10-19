using Microsoft.EntityFrameworkCore;
using UrlShortener.Persistence.Constants;
using UrlShortener.Persistence.DbContexts;

namespace UrlShortener.Services;

public class UrlShorteningService(ApplicationDbContext dbContext)
{
    private readonly Random random = new();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[ShortLinkSettings.Length];
        int maxValue = ShortLinkSettings.Alphabet.Length;

        while (true)
        {
            for (var i = 0; i < ShortLinkSettings.Length; i++)
            {
                var randomIndex = random.Next(maxValue);
                codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
            }

            string code = new(codeChars);

            if (!await dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }
    }
}
