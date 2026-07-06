using VoiceAIAgent.Data;
using VoiceAIAgent.Interfaces;
using VoiceAIAgent.Models;

namespace VoiceAIAgent.Services;

public class ProductService : IProductService
{
    private Product? _lastProduct;

    public Product? FindProduct(string query)
    {
        query = query.ToLower();

        Product? bestProduct = null;
        int bestScore = 0;

        foreach (var product in ProductRepository.Products)
        {
            int score = 0;

            string searchable =
                $"{product.Name} {product.Category} {product.Description} {product.Specifications}"
                .ToLower();

            foreach (var word in query.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (searchable.Contains(word))
                    score++;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestProduct = product;
            }
        }

        return bestProduct;
    }

    public string GetAnswer(string query)
    {
        query = query.ToLower();

        Product? product = FindProduct(query);

        if (product != null)
        {
            _lastProduct = product;
        }
        else
        {
            product = _lastProduct;
        }

        if (product == null)
        {
            return "Sorry, I couldn't find any product matching your request.";
        }

        if (query.Contains("price") || query.Contains("cost"))
        {
            return $"{product.Name} costs ${product.Price}.";
        }

        if (query.Contains("shipping"))
        {
            return "Shipping charges are: " +
                   string.Join(", ",
                       product.ShippingPrices.Select(x => $"{x.Key}: ${x.Value}"));
        }

        if (query.Contains("spec") ||
            query.Contains("feature") ||
            query.Contains("configuration"))
        {
            return $"Specifications:\n{product.Specifications}";
        }

        if (query.Contains("description") ||
            query.Contains("about"))
        {
            return product.Description;
        }

        if (query.Contains("guide") ||
            query.Contains("manual"))
        {
            return product.UserGuide;
        }

        if (query.Contains("battery"))
        {
            return $"{product.Name} offers all-day battery life.";
        }

        if (query.Contains("camera"))
        {
            return $"{product.Name} features a 50MP camera.";
        }

        return $"""
                I found {product.Name}.

                Category: {product.Category}

                Price: ${product.Price}

                Description:
                {product.Description}

                What would you like to know next?

                • Price
                • Specifications
                • Shipping
                • User Guide
                • Camera
                • Battery
                """;
    }
}