using TheLegoProject.Models;

namespace TheLegoProject.Models
{
    public interface ILegoRepository
    {
        IQueryable<Product> Products { get; }
        
        IQueryable<Recommendation> Recommendations { get; }
        IQueryable<CustRecommendation> CustRecommendations { get; }
         IQueryable<Order> Orders { get; }

        // Get a list of all products
        IEnumerable<Product> GetAllProducts();

        // Get a single product by ID
        Product GetProductById(int productId);

        // Add a new product
        void AddProduct(Product product);

        // Update an existing product
        void UpdateProduct(int id, Product product);

        // Delete a product by ID
        void DeleteProduct(int productId);
        
        Order AddOrder(Order order);
        Task SaveChangesAsync();
        
        void SaveChanges(Order newOrder);
       
    }
}