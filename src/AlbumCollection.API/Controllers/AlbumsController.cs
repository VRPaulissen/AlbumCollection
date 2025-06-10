using AlbumCollection.API.Models.DTOs;
using AlbumCollection.Core.Models;
using AlbumCollection.Core.Repositories;
using AlbumCollection.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlbumCollection.API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class AlbumsController(IAlbumRepository repository, IDiscogsService discogs) : ControllerBase
    {
        // GET: api/albums
        // Retrieves all albums as DTOs, with optional filtering and pagination
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAll(
            [FromQuery] string? artist,
            [FromQuery] string? title,
            [FromQuery] long? upc,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = repository.Query();
            if (!string.IsNullOrEmpty(artist)) query = query.Where(a => a.Artist.Contains(artist));
            if (!string.IsNullOrEmpty(title))  query = query.Where(a => a.Title.Contains(title));
            if (upc.HasValue)  query = query.Where(a => a.UPC.Equals(upc));

            var items = await query
                .Include(a => a.Tracks)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(items.Select(ConvertToDto));
        }

        // GET: api/albums/{id}
        // Retrieves a single album DTO by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumDto>> GetById(Guid id)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null) return NotFound();
            return Ok(ConvertToDto(album));
        }

        // POST: api/albums
        // Adds a new album entry to the database
        [HttpPost]
        public async Task<ActionResult<AlbumDto>> CreateAlbum(CreateAlbumDto createDto)
        {
            var album = createDto.ConvertToAlbum();
            await repository.AddAsync(album);
            await repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = album.Id }, ConvertToDto(album));
        }

        /// <summary>
        /// Import an album from Discogs by UPC barcode.
        /// </summary>
        [HttpPost("import/by-upc/{upc}")]
        public async Task<ActionResult<AlbumDto>> ImportByUpc(long upc)
        {
            var album = await discogs.FetchByUpcAsync(upc);
            if (album == null) return BadRequest($"Discogs lookup failed for UPC {upc}");

            await repository.AddAsync(album);
            await repository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = album.Id }, ConvertToDto(album));
        }
        
        // POST: api/albums/bulk-import
        // Bulk import of albums, returns list of DTOs
        [HttpPost("bulk-import")]
        public async Task<IActionResult> BulkImport(List<CreateAlbumDto> dtos)
        {
            var albums = dtos.Select(d => d.ConvertToAlbum()).ToList();
            repository.AddRange(albums);
            await repository.SaveChangesAsync();
            return Created("api/albums", albums.Select(ConvertToDto));
            
        }
        
        // PUT: api/albums/{id}
        // Updates an existing album
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(Guid id, UpdateAlbumDto updateDto)
        {
            var album = await repository.GetByIdAsync(id);
            if (album == null) return NotFound();

            updateDto.ApplyToAlbum(album);
            await repository.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/albums/{id}
        // Removes an album from the database
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            await repository.DeleteAsync(id);
            return NoContent();
        }

        #region Utility
        // Converts an Album to an AlbumDto
        private AlbumDto ConvertToDto(Album album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Artist = album.Artist,
                Genre = album.Genre,
                Style = album.Style,
                Publisher = album.Publisher,
                CoverUrl = album.CoverUrl,
                ReleaseYear = album.ReleaseYear,
                UPC = album.UPC,
                DiscogsId = album.DiscogsId,
                SpotifyUri = album.SpotifyUri,
                DateAdded = album.DateAdded,

                // Map each Track → TrackDto
                Tracks = album.Tracks.Select(ConvertToDto).ToList()
            };
        }

        // Converts an Track to an TrackDto
        private TrackDto ConvertToDto(Track track)
        {
            return new TrackDto()
            {
                Id = track.Id,
                TrackNumber = track.TrackNumber,
                Name = track.Name,
                Duration = track.Duration,
                AlbumId = track.AlbumId,
            };
        }
        #endregion
    }
}
