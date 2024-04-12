using System.Collections;

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
    
    public int TransactionId { get; set; }
    

    public int? Qty { get; set; }

    public int? Rating { get; set; }
    
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? BirthDate { get; set; }

    public string? CountryOfResidence { get; set; }

    public string? Gender { get; set; }

    public double? Age { get; set; }

    public string? Date { get; set; }

    public string? DayOfWeek { get; set; }

    public int? Time { get; set; }

    public string? EntryMode { get; set; }

    public double? Amount { get; set; }

    public string? TypeOfTransaction { get; set; }

    public string? CountryOfTransaction { get; set; }

    public string? ShippingAddress { get; set; }

    public string? Bank { get; set; }

    public string? TypeOfCard { get; set; }

    public int? Fraud { get; set; }
    
    public List<OrderItemViewModel> OrderItems { get; set; }
    
}
public class OrderItemViewModel
{

    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}