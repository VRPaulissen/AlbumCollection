using AlbumCollection.Core.Models;

namespace AlbumCollection.API.Models.DTOs
{
    /// <summary>
    /// DTO for updating an existing album.
    /// All fields are optional; Id is passed via route.
    /// </summary>
    public class UpdateAlbumDto
    {
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Genre { get; set; }
        public string? Style { get; set; }
        public string? Publisher { get; set; }
        public string? CoverUrl { get; set; }
        public int? ReleaseYear { get; set; }
        public string? UPC { get; set; }
        public long? DiscogsId { get; set; }
        public string? SpotifyUri { get; set; }

        public List<TrackDto>? Tracks { get; set; }
        
        /// <summary>
        /// Applies any supplied fields to the given Album instance.
        /// </summary>
        /// <param name="album">The existing album to patch.</param>
        public void ApplyToAlbum(Album album)
        {
            if (Title is not null) album.Title = Title;
            if (Artist is not null) album.Artist = Artist;
            if (Genre is not null) album.Genre = Genre;
            if (Style is not null) album.Style = Style;
            if (Publisher is not null) album.Publisher = Publisher;
            if (CoverUrl is not null) album.CoverUrl = CoverUrl;
            if (ReleaseYear.HasValue) album.ReleaseYear = ReleaseYear.Value;
            if (UPC is not null) album.UPC = UPC;
            if (DiscogsId.HasValue) album.DiscogsId = DiscogsId.Value;
            if (SpotifyUri is not null) album.SpotifyUri = SpotifyUri;
            if (Tracks is not null) album.Tracks = Tracks.Select(t => t.ConvertToTrack()).ToList();
        }
    }
}