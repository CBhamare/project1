using System.ComponentModel.DataAnnotations;

namespace MoviesList.Models.Domain
{
    public class Genre
    {
        public int ID { get; set; }
        [Required]
        public string? GenreName { get; set; }
    }
}
