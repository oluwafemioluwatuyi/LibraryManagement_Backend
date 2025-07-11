using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
