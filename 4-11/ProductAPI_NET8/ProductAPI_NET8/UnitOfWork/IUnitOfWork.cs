using System.Threading.Tasks;
using ProductAPI.Models;
using ProductAPI.Repositories;

namespace ProductAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<ProductCategory> ProductCategories { get; }
        Task<int> SaveAsync();
    }
}
