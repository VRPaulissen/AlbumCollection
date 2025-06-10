using System.Net.Http.Json;
using AlbumCollection.Core.Models;
using AlbumCollection.Core.Services;
using Microsoft.Extensions.Options;

namespace AlbumCollection.Infrastructure.Services;

public class DiscogsService(HttpClient http, IOptions<DiscogsOptions> opts) : IDiscogsService
{
    public async Task<Album?> FetchByUpcAsync(string upc)
    {
        // Search by barcode
        var search = await http.GetFromJsonAsync<DiscogsSearchResult>($"database/search?barcode={upc}");
        var releaseId = search?.Results?.FirstOrDefault()?.Id;
        if (releaseId == null) return null;

        // Fetch full release details
        var release = await http.GetFromJsonAsync<DiscogsRelease>($"releases/{releaseId}");
        if (release == null) return null;

        // Map to your domain Album + Tracks
        var album = new Album
        {
            Id          = Guid.NewGuid(),
            Title       = release.Title,
            Artist      = string.Join(", ", release.Artists.Select(a => a.Name)),
            Genre       = release.Genres.FirstOrDefault() ?? string.Empty,
            Style       = string.Join(", ", release.Styles),
            Publisher   = release.Labels.FirstOrDefault()?.Name ?? string.Empty,
            CoverUrl    = release.Images.FirstOrDefault()?.Uri ?? string.Empty,
            ReleaseYear = release.Year,
            UPC         = upc,
            DiscogsId   = release.Id,
            DateAdded   = DateTime.UtcNow,
            Tracks      = release.Tracklist.Select((t, i) => new Track
            {
                TrackNumber = t.Position is string pos && int.TryParse(pos.Trim('\''), out var n) ? n : i+1,
                Name        = t.Title,
                Duration    = ParseDuration(t.Duration),
                AlbumId     = Guid.Empty // will be set by EF when you add the Album
            }).ToList()
        };
        return album;
    }
    
    private static double ParseDuration(string s)
    {
        var parts = s.Split(':');
        if (parts.Length == 2 
            && int.TryParse(parts[0], out var m) 
            && int.TryParse(parts[1], out var sec))
            return m + sec/60.0;
        return 0;
    }
    
    // JSON DTOs for Discogs response (simplified)
    private record DiscogsSearchResult(Result[]? Results);
    private record Result(int Id);
    private record DiscogsRelease(
        int Id,
        string Title,
        Artist[] Artists,
        string[] Genres,
        string[] Styles,
        Label[] Labels,
        Image[] Images,
        int Year,
        TrackItem[] Tracklist
    );
    private record Artist(string Name);
    private record Label(string Name);
    private record Image(string Uri);
    private record TrackItem(string Position, string Title, string Duration);
}