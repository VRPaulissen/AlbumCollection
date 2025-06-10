using AlbumCollection.Core.Models;
using AlbumCollection.Core.Repositories;
using AlbumCollection.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlbumCollection.Infrastructure.Repositories
{
    public class EfAlbumRepository(AlbumContext context) : IAlbumRepository
    {
        public IQueryable<Album> Query()
        {
            return context.Albums.AsQueryable();
        }

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            return await context.Albums.Include(a => a.Tracks).ToListAsync();
        }
        
        public async Task<Album?> GetByIdAsync(Guid id)
        {
            return await context.Albums.Include(a => a.Tracks).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Album album)
        {
            context.Albums.Add(album);
            await context.SaveChangesAsync();
        }
        
        public void AddRange(IEnumerable<Album> albums)
        {
            context.Albums.AddRange(albums);
        }

        public async Task UpdateAsync(Album album)
        {
            context.Albums.Update(album);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var album = await context.Albums.FindAsync(id);
            if (album != null)
            {
                context.Albums.Remove(album);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAllAsync()
        {
            foreach (var album in context.Albums.ToArray())
            {
                context.Albums.Remove(album);
            }
            
            await context.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}