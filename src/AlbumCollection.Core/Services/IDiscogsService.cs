using AlbumCollection.Core.Models;

namespace AlbumCollection.Core.Services
{
    public interface IDiscogsService
    {
        /// <summary>
        /// Fetches a release from Discogs by UPC code, or null if not found.
        /// </summary>
        Task<Album?> FetchByUpcAsync(long upc);
    }
}