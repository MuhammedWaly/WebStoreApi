using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class UpdateUserProfileDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        public string? Phone { get; set; } = "";

        [Required, MaxLength(100)]
        public string userName { get; set; }

        public string? Address { get; set; } = "";
        
    }
}
