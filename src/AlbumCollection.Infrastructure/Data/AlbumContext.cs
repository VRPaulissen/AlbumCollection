using AlbumCollection.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AlbumCollection.Infrastructure.Data
{
    /// <summary>
    /// EF Core DB context for albums, mapping the Album entity to the Albums table.
    /// </summary>
    public class AlbumContext(DbContextOptions<AlbumContext> options) : DbContext(options)
    {
        // DbSet maps to the Albums table in the database
        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Track> Tracks { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship
            modelBuilder.Entity<Track>().HasKey(t => t.Id);

            modelBuilder.Entity<Track>()
                .HasOne<Album>()                 
                .WithMany(a => a.Tracks)
                .HasForeignKey(t => t.AlbumId);

            base.OnModelCreating(modelBuilder);
        }
    }
}