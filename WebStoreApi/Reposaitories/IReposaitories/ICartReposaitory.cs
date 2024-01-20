using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface ICartReposaitory
    {
        
        Task<CartDto> GetCartASync(string ProducrtIdentifires);
        Dictionary<string, string> GetPaymentMethods();
    }
}
