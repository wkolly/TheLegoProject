using System.Collections;
namespace TheLegoProject.Models.ViewModels;

    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public List<int> PageSizeOptions { get; set; }
    }


