using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class OrderResponseDto
    {

        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }


        public DateTime CreatedAt { get; set; }

        [Precision(16, 2)]
        public decimal ShippingFee { get; set; }

        [MaxLength(100)]
        public string DeliveryAddress { get; set; } = "";

        [MaxLength(30)]
        public string PaymentMethod { get; set; } = "";

        [MaxLength(30)]
        public string PaymentStatus { get; set; } = "";

        [MaxLength(30)]
        public string OrderStatus { get; set; } = "";
       

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
