using System;
using System.Collections.Generic;

namespace TheLegoProject.Models;

public partial class Recommendation
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public int? PopScore { get; set; }

    public string? Rec1 { get; set; }

    public string? Rec2 { get; set; }

    public string? Rec3 { get; set; }

    public string? Rec4 { get; set; }

    public string? Rec5 { get; set; }
}
