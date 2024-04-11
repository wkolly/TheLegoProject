using Microsoft.EntityFrameworkCore;

namespace TheLegoProject.Models;

    public class EFLegoRepository : ILegoRepository
    {
        private LegoDatabase2Context _context;
        
        public EFLegoRepository(LegoDatabase2Context temp)
        {
            _context = temp;
        }

        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Recommendation> Recommendations => _context.Recommendations;
        
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
