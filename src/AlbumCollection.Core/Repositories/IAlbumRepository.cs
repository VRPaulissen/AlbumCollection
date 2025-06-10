using AlbumCollection.Core.Models;

namespace AlbumCollection.Core.Repositories
{
    /// <summary>
    /// Abstraction for CRUD operations on Album entities.
    /// </summary>
    public interface IAlbumRepository
    {
        IQueryable<Album> Query(); 
        Task<IEnumerable<Album>> GetAllAsync();
        Task<Album?> GetByIdAsync(Guid id);
        Task AddAsync(Album album);
        void AddRange(IEnumerable<Album> albums);
        Task UpdateAsync(Album album);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();  
    }
}