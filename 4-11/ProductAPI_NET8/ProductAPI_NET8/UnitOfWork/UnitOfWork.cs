using System.Threading.Tasks;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductAPI.Repositories;

namespace ProductAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRepository<Product> Products { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<ProductCategory> ProductCategories { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new Repository<Product>(_context);
            Categories = new Repository<Category>(_context);
            ProductCategories = new Repository<ProductCategory>(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
