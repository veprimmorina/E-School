using Microsoft.EntityFrameworkCore;
using MultiTenant.Models;
using MultiTenant.Services.Dtos;

namespace MultiTenant.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ProductService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Product CreateProduct(CreatepProductRequest req)
        {
            var product = new Product();
            product.Description = req.Description;
            product.Name = req.Name;

            applicationDbContext.Add(product);
            applicationDbContext.SaveChanges();

            return product;
        }

        public bool DeleteProduct(int id)
        {
            var producttodelete = applicationDbContext.Products.Where(x => x.Id == id).FirstOrDefault();
            applicationDbContext.Products.Remove(producttodelete);
            applicationDbContext.SaveChanges();

            return true;
        }

        public IEnumerable<Product> GetProducts()
        {
            return  applicationDbContext.Products.ToList();
        }
    }
}
