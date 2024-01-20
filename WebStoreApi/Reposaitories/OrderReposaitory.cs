using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStoreApi.Data;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;

namespace WebStoreApi.Reposaitories
{
    public class OrderReposaitory : IOrderReposaitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderReposaitory(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> CreateOrder(OrderDto orderDto, string UserId)
        {

            var productDictoinary = OrderHelper.GetProductDictionary(orderDto.ProductIdentifires);

            Order order = new Order();
            order.UserId = UserId;
            order.PaymentMethod = orderDto.PaymentMethod;
            order.ShippingFee = OrderHelper.ShoppingFee;
            order.DeliveryAddress = orderDto.DeliveryAddress;
            order.PaymentStatus = OrderHelper.PaymentStatues[0];
            order.OrderStatus = OrderHelper.OrderStatues[0];
            order.CreatedAt = DateTime.UtcNow;




            foreach (var pair in productDictoinary)
            {
                int productId = pair.Key;
                var product = await _context.Products.FindAsync(productId);


                var orderItem = new OrderItem();


                orderItem.ProductId = productId;
                orderItem.Quantity = pair.Value;
                orderItem.UnitPrice = product.Price;
                //Order = order,
                //OrderId= order.Id,
                //Product= product,


                order.OrderItems.Add(orderItem);

            }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in order.OrderItems)
            {
                item.Order = null;
            }

            return _mapper.Map<OrderResponseDto>(order);
        }



        public OrderResponseDto GetOrderbyId(string Role, string UserId, int id)
        {
            Order? order = null;

            if (Role == "admin")
            {
                order =  _context.Orders
                                     .Include(o => o.User)
                                     .Include(oi => oi.OrderItems)
                                     .ThenInclude(p => p.Product)
                                     .FirstOrDefault(o => o.Id == id);
            }

            else
            {
                order =  _context.Orders
                                    .Include(o => o.User)
                                    .Include(oi => oi.OrderItems)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefault(o => o.Id == id && o.UserId == UserId);
            }

            foreach (var item in order.OrderItems)
            {
                item.Order = null;
            }
            return  _mapper.Map<OrderResponseDto>(order);
        }



        public async Task<object> GetOrders(string Role, string UserId, int? page)
        {
            IQueryable<Order> query =  _context.Orders
                                     .Include(o => o.User)
                                     .Include(oi => oi.OrderItems)
                                     .ThenInclude(p => p.Product);
            if(Role != "admin")
            {
                query = query.Where(o=>o.UserId==UserId);
            }
            query = query.OrderByDescending(o => o.Id);

            if (page == null || page < 1)
            {
                page = 1;
            }

            int PageSize = 5;
            int TotalPages = 0;

            decimal count = _context.Orders.Count();
            TotalPages = (int)Math.Ceiling(count / PageSize);

            query = query.Skip((int)(page - 1) * PageSize)
                         .Take(PageSize);

            var orders = await query.ToListAsync();

            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Order = null;
                }
            }
             List<OrderResponseDto> listOfOrders =_mapper.Map<List<OrderResponseDto>>(orders);

            var reponse = new
            {
                orders = listOfOrders,
                PageSize = PageSize,
                PageNumber = page,
                TotalPages = TotalPages

            };
            return reponse;
        }

        public async Task<OrderResponseDto> UpdateOrder(string PaumentMethod, string OrderStatues, int id)
        {
            var order = GetOrderByIdForAdmin(id);

            if(PaumentMethod!= null)
            {
                order.PaymentMethod = PaumentMethod;
            }
            
            if(OrderStatues!= null)
            {
                order.OrderStatus = OrderStatues;
            }
            await _context.SaveChangesAsync();

           return  _mapper.Map<OrderResponseDto>(order);
        }
        
        
        public Order GetOrderByIdForAdmin(int id)
        {
            var order = _context.Orders.Find(id);
            return order;
        }

        public async void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
           await _context.SaveChangesAsync();
        }
    }
}
