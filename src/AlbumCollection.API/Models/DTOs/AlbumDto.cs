using AlbumCollection.Core.Models;

namespace AlbumCollection.API.Models.DTOs
{
    /// <summary>
    /// DTO exposed by the API for album data.
    /// Contains all fields the client needs to display.
    /// </summary>
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Style { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string UPC { get; set; } = string.Empty;
        public long DiscogsId { get; set; }
        public string SpotifyUri { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }

        public List<TrackDto> Tracks { get; set; } = new();

    }
}
