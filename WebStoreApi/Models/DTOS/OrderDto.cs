using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class OrderDto
    {
        [Required]
        public string ProductIdentifires {  get; set; }

        [Required,MinLength(10),MaxLength(100)]
        public string DeliveryAddress {  get; set; }

        [MaxLength(100)]
        public string PaymentMethod {  get; set; }
    }
}
