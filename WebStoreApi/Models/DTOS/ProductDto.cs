using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class ProductDto
    {
        [MinLength(1), MaxLength(100),Required]
        public string Name { get; set; }

        [MinLength(1), MaxLength(100),Required]
        public string Brand { get; set; }

        [MinLength(1), MaxLength(100),Required]
        public string Category { get; set; }

        [MaxLength(4000),Required]
        public string Description { get; set; }
    
        public IFormFile? Image { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
