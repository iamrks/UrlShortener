﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Constants;

namespace UrlShortener.Api.Services
{
    public class UrlShorteningService (ApplicationDbContext dbContext)
    {
        private readonly Random random = new();

        public async Task<string> GenerateUnqiueCode()
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
}