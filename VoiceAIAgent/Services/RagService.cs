using VoiceAIAgent.Data;
using VoiceAIAgent.Models;

namespace VoiceAIAgent.Services;

public class RagService
{
    public List<KnowledgeChunk> Retrieve(string query)
    {
        query = query.ToLower();
        query = query
        .Replace("phones", "mobile")
        .Replace("phone", "mobile")
        .Replace("mobiles", "mobile")
        .Replace("cellphone", "mobile");

        List<KnowledgeChunk> chunks = new();

        foreach (var product in ProductRepository.Products)
        {
            string content =
$"""
Product: {product.Name}

Category: {product.Category}

Description:
{product.Description}

Specifications:
{product.Specifications}

User Guide:
{product.UserGuide}
""";

            int score = 0;
            var stopWords = new HashSet<string>
{
    "tell","me","about","show","give","what","is","the",
    "a","an","please","i","want","to","know","can","you"
};

            foreach (var word in query.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                var w = word.ToLower();

                if (stopWords.Contains(w))
                    continue;

                if (w == "phone" || w == "phones" || w == "mobile" || w == "mobiles")
                    w = "mobile";

                if (w == "laptop" || w == "laptops")
                    w = "laptop";

                if (w == "headphone" || w == "headphones")
                    w = "headphones";

                if (w == "watch" || w == "watches")
                    w = "smart watch";

                if (product.Name.ToLower().Contains(w))
                    score += 20;

                if (product.Category.ToLower().Contains(w))
                    score += 15;

                if (product.Description.ToLower().Contains(w))
                    score += 10;

                if (product.Specifications.ToLower().Contains(w))
                    score += 5;

                if (product.UserGuide.ToLower().Contains(w))
                    score += 2;
            }

            chunks.Add(new KnowledgeChunk
            {
                ProductName = product.Name,
                Content = content,
                Score = score
            });
        }

        var bestScore = chunks.Max(x => x.Score);

        return chunks
            .Where(x => x.Score >= bestScore && x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ToList();
    }
}