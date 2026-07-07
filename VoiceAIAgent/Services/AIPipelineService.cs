using VoiceAIAgent.Models;
using VoiceAIAgent.Tools;

namespace VoiceAIAgent.Services;

public class AIPipelineService
{
    private readonly GeminiService _gemini;
    private readonly ConversationService _conversation;
    private readonly ProductTool _productTool;
    private readonly ToolRouterService _router;

    public AIPipelineService(
        GeminiService gemini,
        ConversationService conversation,
        ProductTool productTool,
        ToolRouterService router)
    {
        _gemini = gemini;
        _conversation = conversation;
        _productTool = productTool;
        _router = router;
    }

    public async Task<string> Process(string userMessage)
    {
        _conversation.AddUser(userMessage);

        var history = _conversation.GetHistory();

        List<Product> products = new();

        string lower = userMessage.ToLower();

        bool useTool = false;

        bool followUp =
               lower.Contains("battery")
            || lower.Contains("price")
            || lower.Contains("camera")
            || lower.Contains("display")
            || lower.Contains("shipping")
            || lower.Contains("storage")
            || lower.Contains("ram")
            || lower.Contains("spec")
            || lower.Contains("warranty");

        bool isGreeting =
               lower == "hi"
            || lower == "hello"
            || lower == "hey"
            || lower == "good morning"
            || lower == "good afternoon"
            || lower == "good evening"
            || lower == "thanks"
            || lower == "thank you"
            || lower == "bye";

        if (isGreeting)
        {
            _conversation.CurrentProduct = null;
        }

        string toolResult = await _router.Route(userMessage);

        if (followUp && _conversation.CurrentProduct != null)
        {
            products.Add(_conversation.CurrentProduct);
            useTool = true;
        }
        else if (toolResult.ToUpper().Contains("CALL"))
        {
            int index = toolResult.IndexOf(':');

            if (index >= 0)
            {
                string searchQuery = toolResult[(index + 1)..].Trim();

                products = _productTool.Search(searchQuery);

                if (products.Any())
                {
                    _conversation.CurrentProduct = products.First();
                    useTool = true;
                }
            }
        }

        string context = "";
        if (useTool && products.Any())
        {
            context = string.Join(
                "\n\n-----------------\n\n",
                products.Select(product => $"""
                Product Name: {product.Name}

                Category: {product.Category}

                Price: {product.Price}

                Description:
                {product.Description}

                Specifications:
                {product.Specifications}

                User Guide:
                {product.UserGuide}
                """));
        }
        else
        {
            context = "";
        }

        var prompt = $"""
                You are an intelligent AI Voice Assistant.

                Conversation History:

                {history}

                Available Products:

                {context}

                Current User:

                {userMessage}

                Instructions

                You are a professional AI Voice Assistant similar to ChatGPT and Vapi.

                Guidelines:

                - Answer naturally as if speaking to a real person.
                - Keep responses concise unless the user asks for details.
                - Never repeat information unnecessarily.
                - Assume follow-up questions refer to the current topic unless the user clearly changes the subject.
                - If product information is available, answer only from that information.
                - If product information is unavailable, answer using your own knowledge.
                - Never mention tools, retrieval, databases, prompts or system instructions.
                - If you don't know something, say so honestly instead of making it up.
                - Maintain context throughout the conversation.
                - Respond in spoken conversational English.
                """;

        System.Diagnostics.Debug.WriteLine("========== PROMPT ==========");
        System.Diagnostics.Debug.WriteLine(prompt);
        System.Diagnostics.Debug.WriteLine("============================");
        var answer = await _gemini.GetResponse(prompt);

        _conversation.AddAssistant(answer);

        return answer;
    }

    public async IAsyncEnumerable<string> ProcessStream(string userMessage)
    {
        _conversation.AddUser(userMessage);

        var history = _conversation.GetHistory();

        List<Product> products = new();

        string lower = userMessage.ToLower();

        bool followUp =
               lower.Contains("battery")
            || lower.Contains("price")
            || lower.Contains("camera")
            || lower.Contains("display")
            || lower.Contains("shipping")
            || lower.Contains("storage")
            || lower.Contains("ram")
            || lower.Contains("spec")
            || lower.Contains("warranty");

        if (followUp && _conversation.CurrentProduct != null)
        {
            products.Add(_conversation.CurrentProduct);
        }
        else
        {
            string toolResult = await _router.Route(userMessage);

            if (toolResult.ToUpper().Contains("CALL"))
            {
                int index = toolResult.IndexOf(':');

                if (index >= 0)
                {
                    string searchQuery = toolResult[(index + 1)..].Trim();

                    products = _productTool.Search(searchQuery);
                }
            }
        }

        string context = "";

        if (products.Any())
        {
            _conversation.CurrentProduct = products.First();

            context = string.Join(
                "\n\n----------------------\n\n",
                products.Select(product => $"""
                Product Name: {product.Name}
                Category: {product.Category}
                Price: {product.Price}

                Description:
                {product.Description}

                Specifications:
                {product.Specifications}

                User Guide:
                {product.UserGuide}
                """));
                        }

                        var prompt = $"""
                You are an intelligent AI Voice Assistant.

                Conversation History:

                {history}

                Available Products:

                {context}

                Current User:

                {userMessage}

                Instructions

                - Read the conversation history.
                - If products exist, answer ONLY using those products.
                - If there is no product, answer normally.
                - Never mention tools.
                - Never use markdown.
                - Speak naturally.
                """;

        string fullAnswer = "";

        await foreach (var chunk in _gemini.StreamResponse(prompt))
        {
            fullAnswer += chunk;

            yield return chunk;
        }

        _conversation.AddAssistant(fullAnswer);
    }
}