using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WebStoreApi.Models
{
    public class Order
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
        public ApplicationUser User {  get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    }
}