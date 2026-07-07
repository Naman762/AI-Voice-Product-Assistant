using VoiceAIAgent.Models;

namespace VoiceAIAgent.Services;

public class ConversationService
{
    private readonly List<(string Role, string Text)> _messages = new();

    public Product? CurrentProduct { get; set; }

    public void AddUser(string message)
    {
        _messages.Add(("User", message));
    }

    public void AddAssistant(string message)
    {
        _messages.Add(("Assistant", message));
    }

    public string GetHistory()
    {
        return string.Join(
            "\n",
            _messages.Select(x => $"{x.Role}: {x.Text}")
        );
    }

    public void Clear()
    {
        _messages.Clear();
        CurrentProduct = null;
    }
}