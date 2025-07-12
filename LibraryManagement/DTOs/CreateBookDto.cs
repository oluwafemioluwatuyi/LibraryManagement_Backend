using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
