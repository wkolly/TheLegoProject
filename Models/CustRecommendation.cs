using System;
using System.Collections.Generic;

namespace TheLegoProject.Models;

public partial class CustRecommendation
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Rec1 { get; set; }

    public string? Rec2 { get; set; }

    public string? Rec3 { get; set; }

    public string? Rec4 { get; set; }

    public string? Rec5 { get; set; }
}
