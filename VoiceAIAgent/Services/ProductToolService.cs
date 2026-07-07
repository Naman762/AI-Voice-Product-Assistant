using VoiceAIAgent.Data;
using VoiceAIAgent.Models;

namespace VoiceAIAgent.Services;

public class ProductToolService
{
    public Product? SearchProduct(string query)
    {
        query = query.ToLower();

        return ProductRepository.Products
            .OrderByDescending(p =>
            {
                int score = 0;

                if (p.Name.ToLower().Contains(query))
                    score += 20;

                if (query.Contains("samsung") &&
                    p.Name.ToLower().Contains("galaxy"))
                    score += 15;

                if (query.Contains("iphone") &&
                    p.Name.ToLower().Contains("iphone"))
                    score += 15;

                if (query.Contains("apple") &&
                    p.Name.ToLower().Contains("apple"))
                    score += 10;

                if (p.Category.ToLower().Contains(query))
                    score += 5;

                return score;
            })
            .FirstOrDefault();
    }
}