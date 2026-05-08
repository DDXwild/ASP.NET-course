using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(16, 100)]
        public int Age { get; set; }

        [Required]
        [StringLength(100)]
        public string Major { get; set; } = string.Empty;
    }
}
