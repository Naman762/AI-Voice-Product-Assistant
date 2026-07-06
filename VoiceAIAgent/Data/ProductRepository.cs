using VoiceAIAgent.Models;

namespace VoiceAIAgent.Data;

public static class ProductRepository
{
    public static List<Product> Products = new()
    {
        new Product
        {
            Id = 3,
            Name = "iPhone 16",
            Category = "Mobile",
            Price = 999,
            Description = "Apple flagship smartphone",
            Specifications = "256GB Storage, A18 Chip",
            UserGuide = "Use MagSafe charger for best experience.",
            ShippingPrices = new() {{"USA",0},{"India",25},{"Europe",20}}
        },

        new Product
        {
            Id = 4,
            Name = "Dell XPS 15",
            Category = "Laptop",
            Price = 1799,
            Description = "Premium Windows laptop",
            Specifications = "32GB RAM, 1TB SSD",
            UserGuide = "Keep BIOS updated.",
            ShippingPrices = new() {{"USA",10},{"India",35},{"Europe",25}}
        },

        new Product
        {
            Id = 5,
            Name = "Sony WH-1000XM5",
            Category = "Headphones",
            Price = 399,
            Description = "Noise cancelling wireless headphones",
            Specifications = "Bluetooth 5.3, 30 Hours Battery",
            UserGuide = "Download Sony Headphones app.",
            ShippingPrices = new() {{"USA",5},{"India",15},{"Europe",10}}
        },

        new Product
        {
            Id = 6,
            Name = "Apple Watch Series 10",
            Category = "Smart Watch",
            Price = 499,
            Description = "Advanced smartwatch with health tracking",
            Specifications = "GPS + Cellular",
            UserGuide = "Pair with an iPhone.",
            ShippingPrices = new() {{"USA",5},{"India",20},{"Europe",15}}
        },

        new Product
        {
            Id = 7,
            Name = "iPad Pro M4",
            Category = "Tablet",
            Price = 1199,
            Description = "Professional tablet",
            Specifications = "13-inch OLED Display",
            UserGuide = "Supports Apple Pencil Pro.",
            ShippingPrices = new() {{"USA",0},{"India",20},{"Europe",15}}
        },

        new Product
        {
            Id = 8,
            Name = "Galaxy S25",
            Category = "Mobile",
            Price = 899,
            Description = "Samsung flagship smartphone powered by Snapdragon 8 Elite.",
            Specifications = "12GB RAM, 256GB Storage, 6.2-inch AMOLED Display, 50MP Camera",
            UserGuide = "Charge fully before first use and install the latest software updates.",
            ShippingPrices = new(){{"USA",15},{"India",8},{"Europe",12}}
        }
    };
}