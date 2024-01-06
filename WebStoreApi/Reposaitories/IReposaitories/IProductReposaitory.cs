using WebStoreApi.Models.DTOS;
using WebStoreApi.Models;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface IProductReposaitory
    {
        Task<ProductsPaginationDto<Product>> GetAllProductsAsync(string? search, string? category, int? MinPrice, int? MaxPrice, string? sort, string? order, int? page);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> UpdateProductAsync(int id, ProductDto Dto);
        Task<Product> AddProductAsync(Product Dto);
        Task<bool> DeleteProductAsync(int id);
        List<string> GetAllCategories();
    }
}
