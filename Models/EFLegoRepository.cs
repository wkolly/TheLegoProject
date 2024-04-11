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

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            var productInDb = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (productInDb != null)
            {
                // Update only the specified fields
                productInDb.Year = updatedProduct.Year;
                productInDb.NumParts = updatedProduct.NumParts;
                productInDb.Price = updatedProduct.Price;
                productInDb.PrimaryColor = updatedProduct.PrimaryColor;
                productInDb.SecondaryColor = updatedProduct.SecondaryColor;
                productInDb.Category = updatedProduct.Category;
                productInDb.Description = updatedProduct.Description;
                productInDb.Subcategory = updatedProduct.Subcategory;
                productInDb.ImgLink = updatedProduct.ImgLink;
                
                _context.SaveChanges();
            }
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
