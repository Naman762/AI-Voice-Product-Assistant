using VoiceAIAgent.Data;
using VoiceAIAgent.Models;

namespace VoiceAIAgent.Services;

public class RagService
{
    public List<KnowledgeChunk> Retrieve(string query)
    {
        query = query.ToLower();

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

            foreach (var word in query.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                string w = word.ToLower();

                if (product.Name.ToLower().Contains(w))
                    score += 10;

                if (product.Category.ToLower().Contains(w))
                    score += 6;

                if (product.Description.ToLower().Contains(w))
                    score += 3;

                if (product.Specifications.ToLower().Contains(w))
                    score += 2;

                if (product.UserGuide.ToLower().Contains(w))
                    score += 1;
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