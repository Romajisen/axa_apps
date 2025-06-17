using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ProductApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ProductRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _cache.GetOrCreateAsync($"product_{id}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return _context.Products.FindAsync(id).AsTask();
            });
        }
        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _cache.Remove("all_products"); // invalidate
            return product;
        }

       public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _cache.Remove($"product_{id}");
                _cache.Remove("all_products");
            }
        }

        // ✅ Benar
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }


        public async Task<IEnumerable<Product>> SearchAsync(string? name, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null)
                throw new InvalidOperationException("Product not found");

            existing.Name = product.Name;
            existing.Price = product.Price;

            await _context.SaveChangesAsync();

            _cache.Remove($"product_{product.Id}");
            _cache.Remove("all_products");
        }


    }
}
