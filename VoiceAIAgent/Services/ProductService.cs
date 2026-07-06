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
                $"{product.Name} " +
                $"{product.Category} " +
                $"{product.Description} " +
                $"{product.Specifications}";

            searchable = searchable.ToLower();

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

        return bestScore == 0 ? null : bestProduct;
    }

    public string GetAnswer(string query)
    {
        query = query.ToLower();

        Product? product = FindProduct(query);

        if (product != null)
            _lastProduct = product;

        product ??= _lastProduct;

        if (product == null)
            return "Sorry, I couldn't find that product. Please tell me the product name.";

        if (query.Contains("price"))
            return $"{product.Name} costs ${product.Price}.";

        if (query.Contains("shipping"))
        {
            string shipping = string.Join(", ",
                product.ShippingPrices.Select(x => $"{x.Key}: ${x.Value}"));

            return $"Shipping costs are: {shipping}.";
        }

        if (query.Contains("guide"))
            return $"User Guide: {product.UserGuide}";

        if (query.Contains("camera"))
            return $"{product.Name} has these specifications: {product.Specifications}";

        if (query.Contains("battery"))
            return $"{product.Name} specifications include: {product.Specifications}";

        if (query.Contains("storage"))
            return $"{product.Name} comes with {product.Specifications}.";

        if (query.Contains("spec"))
            return $"{product.Specifications}";

        return
$"""
Sure!

Here's what I found about the {product.Name}.

📱 Category:
{product.Category}

💲 Price:
${product.Price}

📝 Description:
{product.Description}

⚙ Specifications:
{product.Specifications}

🚚 Shipping:
{string.Join(", ", product.ShippingPrices.Select(x => $"{x.Key}: ${x.Value}"))}

Would you like to know about its price, shipping, specifications, or user guide?
""";
    }
}