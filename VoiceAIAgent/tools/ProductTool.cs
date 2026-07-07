using VoiceAIAgent.Data;
using VoiceAIAgent.Models;

namespace VoiceAIAgent.Tools;

public class ProductTool
{
    public List<Product> Search(string query)
    {
        query = query.ToLower();

        var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return ProductRepository.Products
            .Where(product =>
            {
                var text =
                    $"{product.Name} {product.Category} {product.Description} {product.Specifications}"
                    .ToLower();

                return words.Any(w => text.Contains(w));
            })
            .ToList();
    }
}