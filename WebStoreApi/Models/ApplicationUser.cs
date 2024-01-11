using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models
{
    [Index("Email",IsUnique =true)]
    public class ApplicationUser: IdentityUser
    {

        [MaxLength(100)]
        public string FristName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }

        
        [MaxLength(100)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
