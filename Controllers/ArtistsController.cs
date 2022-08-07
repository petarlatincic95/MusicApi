using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System.Linq;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private  ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)  //Atribute is not [FromBody] Because we pick up data form form-data.
        {


            var imageUrl = await FileHelper.UploadImageAsync(artist.Image);
            artist.ImageUrl = imageUrl;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);


        }
        [HttpGet]

        public async Task<IActionResult> GetArtist(int? pageNumber, int? pageSize)          //This Method returns list of albums
        {
            
            
            int currentPageNumber = pageNumber ?? 1;               //We declare this variables as nullable if someone forget to provide us this values
                                                                    
            int currentPageSize = pageSize ?? 5;                  // and store them in current page and size variables to display default settings

            //LINQ quuerry to select only desired fields from Artist class, we don't
            //want to display all fields from Artist class
            var artists = await (from artist in _dbContext.Artists    
            select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl,

            }).ToListAsync();
            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize)); 
        }

        [HttpGet("[action]")]                                                   //We use [action] token becaue our method name doesn't start with Get word

        public async Task<IActionResult> ArtistDetails(int artisdId)           //This method returns etails of album with given id
        {
            var artistDetails = await _dbContext.Artists.Where(a => a.Id == artisdId).Include(a => a.Songs).ToListAsync();  //We include data from Songs table along with Artist table data. We are able to do this becaue we defined 1-->M relationship between artist and song.
            return Ok(artistDetails);
        }

    }
}
