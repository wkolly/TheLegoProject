using System.Collections;
namespace TheLegoProject.Models.ViewModels;

    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public List<int> PageSizeOptions { get; set; }

        // New properties for filtering
        public string SelectedCategory { get; set; }
        public string SelectedColor { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Colors { get; set; }
    }