using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class UserProfileDto
    {
        [Required,MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }
        [Required]
        public string userName { get; set; }
        
        public string? Address { get; set; }
        public string? Message { get; set; } = "";

        public List<string> role { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
