namespace TheLegoProject.Models.ViewModels;

public class ProductRecommendationViewModel
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    
    public string? ImgLink { get; set; }
    public int? Price { get; set; }
    
    public int? PopScore { get; set; }
    
    public string? Rec1 { get; set; }

    public string? Rec2 { get; set; }

    public string? Rec3 { get; set; }

    public string? Rec4 { get; set; }

    public string? Rec5 { get; set; }
    
    public string? PrimaryColor { get; set; }

    public string? SecondaryColor { get; set; }

    public string? Description { get; set; }

    public string? Category { get; set; }

    public string? Subcategory { get; set; }
    
    public int? Year { get; set; }

    public int? NumParts { get; set; }
    
    public string Rec1ImgLink { get; set; }
    public string Rec2ImgLink { get; set; }
    public string Rec3ImgLink { get; set; }
    public string Rec4ImgLink { get; set; }
    public string Rec5ImgLink { get; set; }
}