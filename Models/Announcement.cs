using System.ComponentModel.DataAnnotations;

namespace Announcement_Test_Task.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
