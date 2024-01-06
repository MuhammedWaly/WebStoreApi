using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [MinLength(1), MaxLength(100)]
        public string FirstName { get; set; }
        
        [MinLength(1), MaxLength(100)]
        public string LastName { get; set; }
        
        [MinLength(1), MaxLength(100)]
        public string Email { get; set; }

        [MinLength(1), MaxLength(100)]
        public string Phone { get; set; }

        
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }

        
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
