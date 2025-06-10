using System.ComponentModel.DataAnnotations;
using AlbumCollection.Core.Models;

namespace AlbumCollection.API.Models.DTOs
{
    /// <summary>
    /// DTO for creating a new album.
    /// Excludes Id and DateAdded (server-generated).
    /// </summary>
    public class CreateAlbumDto
    {
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Artist { get; set; } = string.Empty;
        [Required] public string Genre { get; set; } = string.Empty;
        [Required] public string Style { get; set; } = string.Empty;
        [Required] public string Publisher { get; set; } = string.Empty;
        [Required] public string CoverUrl { get; set; } = string.Empty;
        [Required] public int ReleaseYear { get; set; }
        [Required] public long UPC { get; set; }
        public long DiscogsId { get; set; }
        public string SpotifyUri { get; set; } = string.Empty;

        public List<TrackDto> Tracks { get; set; } = new();

        /// <summary>
        /// Converts a CreateAlbumDto to an Album instance.
        /// </summary>
        /// <returns></returns>
        public Album ConvertToAlbum()
        {
            var albumId = Guid.NewGuid();
            return new Album
            {
                Id = albumId,
                Title = Title,
                Artist = Artist,
                Genre = Genre,
                Style = Style,
                Publisher = Publisher,
                CoverUrl = CoverUrl,
                ReleaseYear = ReleaseYear,
                UPC = UPC,
                DiscogsId = DiscogsId,
                SpotifyUri = SpotifyUri,
                DateAdded = DateTime.UtcNow,
                Tracks = Tracks.Select(t =>
                {
                    var track = t.ConvertToTrack();
                    track.AlbumId = albumId;
                    return track;
                }).ToList()
            };
        }
    }
}