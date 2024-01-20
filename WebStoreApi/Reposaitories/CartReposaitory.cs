using WebStoreApi.Data;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;

namespace WebStoreApi.Reposaitories
{
    public class CartReposaitory : ICartReposaitory
    {
        private readonly ApplicationDbContext _context;

        public CartReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> GetCartASync(string ProducrtIdentifires)
        {
            CartDto cartDto = new CartDto();
            cartDto.Items = new List<CartItemDto>();
            cartDto.SubTotal = 0;
            cartDto.ShippingFee = OrderHelper.ShoppingFee;
            cartDto.TotalPrice = 0;

            var productDictionary = OrderHelper.GetProductDictionary(ProducrtIdentifires);

            foreach (var pair in productDictionary)
            {
                int productId = pair.Key;
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                    continue;
                var cartItemDto = new CartItemDto();
                cartItemDto.Product = product;
                cartItemDto.Quantity = pair.Value;

                cartDto.Items.Add(cartItemDto);
                cartDto.SubTotal = product.Price * pair.Value;
                cartDto.TotalPrice = cartDto.SubTotal + cartDto.ShippingFee;
            }
            return (cartDto);

        }

        public Dictionary<string,string> GetPaymentMethods ()
        {
            return (OrderHelper.PaymentMethods);
        }
    }
}
