namespace AlbumCollection.Core.Models
{
    /// <summary>
    /// Represents a music album in our collection.
    /// </summary>
    public class Album
    {
        // General Data
        public Guid Id { get; set; }   
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Style { get; set; }
        public string Publisher { get; set; }
        public string CoverUrl { get; set; }     
        public int ReleaseYear { get; set; }  
        public string UPC { get; set; }
        
        // API References
        public long DiscogsId { get; set; }
        public string SpotifyUri { get; set; } = string.Empty;
        

        // Track list for this album
        public List<Track> Tracks { get; set; } = new();
        
        // Other
        public DateTime DateAdded { get; set; } 
    }
}