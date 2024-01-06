using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class UserDto
    {
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        [MinLength(8),MaxLength(100)]
        public string Password { get; set; }
    }
}
