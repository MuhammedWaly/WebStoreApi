using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MinLength(1), MaxLength(100)]
        public string Name { get; set; }

        [MinLength(1), MaxLength(100)]
        public string Brand {  get; set; }

        [MinLength(1), MaxLength(100)]
        public string Category {  get; set; }
        
        
        public string Description {  get; set; }

        [MinLength(1), MaxLength(100)]
        public string Image {  get; set; }
        [Precision(16,2)]
        public decimal Price {  get; set; }  
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
    }
}
