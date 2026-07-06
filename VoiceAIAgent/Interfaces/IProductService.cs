using VoiceAIAgent.Models;

namespace VoiceAIAgent.Interfaces;

public interface IProductService
{
    Product? FindProduct(string query);

    string GetAnswer(string query);
}