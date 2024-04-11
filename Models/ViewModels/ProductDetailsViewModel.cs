using TheLegoProject.Models.ViewModels;

public class ProductDetailsViewModel
{
    public ProductRecommendationViewModel Product { get; set; }
    public List<ProductRecommendationViewModel> RelatedProducts { get; set; }
}

