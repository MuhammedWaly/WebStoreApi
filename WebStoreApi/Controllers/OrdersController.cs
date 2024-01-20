using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebStoreApi.Data;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAccountReposaitory _accountReposaitory;
        private readonly IOrderReposaitory _orderReposaitory;
        private readonly IProductReposaitory _productReposaitory;

        public OrdersController(IAccountReposaitory accountReposaitory, IOrderReposaitory orderReposaitory, IProductReposaitory productReposaitory)
        {
            _accountReposaitory = accountReposaitory;
            _orderReposaitory = orderReposaitory;
            _productReposaitory = productReposaitory;
        }

        [Authorize]
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders (int? page)
        {
            string UserId = JwtReader.GetUserId(User);
            string Role = JwtReader.GetUserRole(User);

            var orders = await _orderReposaitory.GetOrders(Role, UserId, page);
            return Ok(orders);
        }
        
        
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetOrderbyId (int id)
        {
            string UserId = JwtReader.GetUserId(User);
            string Role = JwtReader.GetUserRole(User);

            var order =  _orderReposaitory.GetOrderbyId(Role, UserId, id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            if (!OrderHelper.PaymentMethods.ContainsKey(orderDto.PaymentMethod))
            {
                ModelState.AddModelError("PaymentMethod", "Please select a valid payment method");
                return BadRequest(ModelState);
            }
            string userId = JwtReader.GetUserId(User);
            var user = await _accountReposaitory.FindUserById(userId);
            if (user==null)
            {

                ModelState.AddModelError("Order", "Unable to create the Order");
                return BadRequest(ModelState);
            }

            var productDictoinary = OrderHelper.GetProductDictionary(orderDto.ProductIdentifires);

            foreach (var pair in productDictoinary)
            {
                int productId = pair.Key;
                var product = await _productReposaitory.GetProductByIdAsync(productId);
                if (product == null)
                {
                    ModelState.AddModelError("Product", "No Product with this Id"+ productId +" ");
                }
              
            }

            var order = await _orderReposaitory.CreateOrder(orderDto, userId);

            return Ok(order);

        }

        [Authorize(Roles ="admin")]
        [HttpPut("UpdateOrder{id}")]
        public async Task<IActionResult> UpdateOrder(int id, string PaymentMethod, string OrderStatues)
        {
            if (PaymentMethod == null && OrderStatues == null)
            {
                ModelState.AddModelError("Update Order", "there is nothing to update");
                return BadRequest(ModelState);
            }

            if (PaymentMethod!= null && !OrderHelper.PaymentMethods.ContainsKey(PaymentMethod))
            {
                ModelState.AddModelError("Payment Method", "Please select a valid payment method");
                return BadRequest(ModelState);
            }
            
            if (OrderStatues != null && !OrderHelper.OrderStatues.Contains(OrderStatues))
            {
                ModelState.AddModelError("Order statues", "Please select a valid order statues");
                return BadRequest(ModelState);
            }

             
            if (_orderReposaitory.GetOrderByIdForAdmin(id) == null)
                return NotFound();

             var order = await _orderReposaitory.UpdateOrder(PaymentMethod, OrderStatues,id);

            return Ok(order);

        }
        [Authorize(Roles ="admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder (int id)
        {
            var order = _orderReposaitory.GetOrderByIdForAdmin(id);
            if (order == null)
                return NotFound();
             _orderReposaitory.DeleteOrder(order);
            return Ok();
        }


    }
}
/*  eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IkFAQS5jb20iLCJyb2xlIjoiYWRtaW4iLCJVSWQiOiIxZDMyZDM4Yi05ZTAzLTQ5NDEtOTFjZi1kZDc4NzZjYjA1MjciLCJuYmYiOjE3MDU3NTkwNTksImV4cCI6MTcwNjM2Mzg1OCwiaWF0IjoxNzA1NzU5MDU5fQ.npcUTn75d-hWBCB5U9FVy5hXin5EFDn7fU5jrRR8OOM   */

