using System.ComponentModel.DataAnnotations;

namespace StudentCrudApp.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        // Optional field for Profile Picture stored as byte[]
        public byte[]? ProfilePicture { get; set; }

        // Navigation property for related addresses
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
