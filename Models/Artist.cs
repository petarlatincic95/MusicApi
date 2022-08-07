using System.ComponentModel.DataAnnotations.Schema;

namespace MusicApi.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }

        [NotMapped]                                 //We don't want this property to be in our db table
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Album>? Albums { get; set; }
        public ICollection<Song>? Songs { get; set; }

    }
}
