using System.ComponentModel.DataAnnotations;

namespace AlbumCollection.Core.Models
{
    /// <summary>
    /// Represents a track of an album in our collection.
    /// </summary>
    public class Track
    {
        [Key]
        public int Id { get; set; }
        public int TrackNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; }
        public Guid AlbumId { get; set; }
    }
}