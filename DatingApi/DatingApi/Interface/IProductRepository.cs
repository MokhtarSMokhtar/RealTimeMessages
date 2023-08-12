using ProductDAL.Entities;

namespace DatingApi.Interface
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetProducts();
    }
}
