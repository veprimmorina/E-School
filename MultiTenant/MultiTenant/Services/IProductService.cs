using MultiTenant.Models;
using MultiTenant.Services.Dtos;

namespace MultiTenant.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product CreateProduct (CreatepProductRequest product);
        bool DeleteProduct (int id);
    }
}
