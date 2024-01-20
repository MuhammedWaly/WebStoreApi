namespace WebStoreApi.Models.DTOS
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
