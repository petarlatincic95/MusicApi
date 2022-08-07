using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {

            int currentPageNumber = pageNumber ?? 1;     //We declare this variables as nullable if someone forget to provide us this values
                                                         // and store them in current page and size variables to display default settings
            int currentPageSize = pageSize ?? 5;
            var songs = await (from Song in _dbContext.Songs
                               select new
                               {   Id= Song.Id,                         //Anonimous type variables
                                   Title = Song.Title,
                                   Duration = Song.Duration,
                                   Uploaded = Song.UploadedDate,
                                   ImageUrl = Song.ImageUrl,
                                   AudioUrl = Song.AudioUrl,

                               }).ToListAsync();
            
               return Ok(songs.Skip((currentPageNumber-1)*currentPageSize).Take(currentPageSize));    
        
        
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from Song in _dbContext.Songs
                        where Song.IsFeatured==true
                        select new
                        {
                            Id = Song.Id,                         //Anonimous type variables
                            Title = Song.Title,
                            Duration = Song.Duration,
                            Uploaded = Song.UploadedDate,
                            ImageUrl = Song.ImageUrl,
                            AudioUrl = Song.AudioUrl,
                            

                        }).ToListAsync();
            return Ok(songs);
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from Song in _dbContext.Songs
                               orderby Song.UploadedDate descending
                               select new
                               {
                                   Id = Song.Id,                         //Anonimous type variables
                                   Title = Song.Title,
                                   Duration = Song.Duration,
                                   Uploaded = Song.UploadedDate,
                                   ImageUrl = Song.ImageUrl,
                                   AudioUrl = Song.AudioUrl,


                               }).Take(10).ToListAsync();          //We want to take only first 10 newest songs
            return Ok(songs);

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string querry)
        {
            var songs = await (from Song in _dbContext.Songs
                              where Song.Title.StartsWith(querry)
                              select new
                               {
                                   Id = Song.Id,                         //Anonimous type variables
                                   Title = Song.Title,
                                   Duration = Song.Duration,
                                   Uploaded = Song.UploadedDate,
                                   ImageUrl = Song.ImageUrl,
                                   AudioUrl = Song.AudioUrl,


                               }).Take(10).ToListAsync();          //We want to take only first 10 newest songs
            return Ok(songs);

        }





        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)  //Atribute is not [FromBody] Because we pick up data form form-data.
        {


            var imageUrl = await FileHelper.UploadImageAsync(song.Image);
            song.ImageUrl = imageUrl;
            var audioUrl = await FileHelper.UploadFileAsync(song.AudioFile);
            song.AudioUrl = audioUrl;
            song.UploadedDate=DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);


        }
    }
}
