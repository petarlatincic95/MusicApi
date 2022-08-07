using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public DateTime UploadedDate { get; set; }
        public bool IsFeatured { get; set; }



        [NotMapped]                             //We don't want this property to be in our db table
        public IFormFile Image { get; set; }
        
        public  string? ImageUrl { get; set; }  //We declare this property as nullable to not get 400 bad request response when
                                                //calling post method and we send it as empty key in post method.
        [NotMapped]                             //We don't want this property to be in our db table
        public IFormFile AudioFile { get; set; }
        public string? AudioUrl { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }  //We set this property as nullable because some artists don't have albums but anyway they can have songs.



        //public string TestAtribute { get; set; }

    }
      
}
