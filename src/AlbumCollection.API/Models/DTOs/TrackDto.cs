using AlbumCollection.Core.Models;

namespace AlbumCollection.API.Models.DTOs
{
    /// <summary>
    /// DTO representing a track to send to/from the client.
    /// </summary>
    public class TrackDto
    {
        public int Id { get; set; }
        public int TrackNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; }
        public Guid AlbumId { get; set; }

        /// <summary>
        /// Converts a TrackDto to a Track
        /// </summary>
        /// <returns></returns>
        public Track ConvertToTrack()
        {
            return new Track()
            {
                Id = Id,
                TrackNumber = TrackNumber,
                Name = Name,
                Duration = Duration,
                AlbumId = AlbumId
            };
        }
    }
}