namespace VoiceAIAgent.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Category { get; set; } = "";

    public decimal Price { get; set; }

    public string Description { get; set; } = "";

    public string Specifications { get; set; } = "";

    public string UserGuide { get; set; } = "";

    public Dictionary<string, decimal> ShippingPrices { get; set; } = new();
}