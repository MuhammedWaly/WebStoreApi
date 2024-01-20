using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface IOrderReposaitory
    {
        Task<object> GetOrders (string Role,string UserId,int? page);
        //Task<Order> GetOrderbyId (string Role,string UserId,int id);
        Task<OrderResponseDto> CreateOrder(OrderDto orderDto, string UserId);
        Task<OrderResponseDto> UpdateOrder(string PaumentMethod, string OrderStatues, int id);
        OrderResponseDto GetOrderbyId(string Role, string UserId, int id);
        Order GetOrderByIdForAdmin(int id);
        void DeleteOrder(Order order);
        //Task<Order> (OrderDto orderDto, string UserId, Product product);
    }
}
