using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)  //Atribute is not [FromBody] Because we pick up data form form-data.
        {


            var imageUrl = await FileHelper.UploadImageAsync(album.Image);
            album.ImageUrl = imageUrl;
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);


        }
        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
         {
            int currentPageNumber = pageNumber ?? 1;     //We declare this variables as nullable if someone forget to provide us this values
                                                         
            int currentPageSize = pageSize ?? 5;         // and store them in current page and size variables to display default settings

            var albums = await (from album in _dbContext.Albums  //Linq querry to choose and return only fields we want our user to see about albums
                                 select new
                                 {
                                     Id = album.Id,
                                     Name = album.Name,
                                     ImageUrl = album.ImageUrl,

                                 }).ToListAsync();
           return Ok(albums.Skip((currentPageNumber-1)*currentPageSize).Take(currentPageSize));    


        }
        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumDetails = await _dbContext.Albums.Where(a=>a.Id==albumId).Include(a => a.Songs).ToListAsync();
            return Ok(albumDetails);
        
        }
    }
}
