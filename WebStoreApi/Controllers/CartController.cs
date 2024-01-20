using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartReposaitory _cartReposaitory;

        public CartController(ICartReposaitory cartReposaitory)
        {
            _cartReposaitory = cartReposaitory;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCart(string ProductIdentifires)
        {
            var cart = await _cartReposaitory.GetCartASync(ProductIdentifires);

            return Ok(cart);
        }
        
        [HttpGet("GetPaymentMethods")]
        public  IActionResult GetPaymentMethods()
        {
            var PaymentMethods = _cartReposaitory.GetPaymentMethods();

            return Ok(PaymentMethods);
        }
    }
}
