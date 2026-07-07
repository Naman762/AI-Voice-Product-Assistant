namespace VoiceAIAgent.Services;

public class ToolRouterService
{
    public Task<string> Route(string message)
    {
        string text = message.ToLower();

        string[] productKeywords =
        {
            "iphone","apple",
            "samsung","galaxy",
            "pixel",
            "oneplus",
            "xiaomi",
            "oppo",
            "vivo",
            "realme",
            "motorola",
            "nokia",

            "dell",
            "hp",
            "lenovo",
            "asus",
            "acer",
            "macbook",

            "watch",
            "tablet",
            "phone",
            "mobile",
            "laptop",

            "battery",
            "camera",
            "display",
            "screen",
            "price",
            "cost",
            "shipping",
            "delivery",
            "warranty",
            "ram",
            "storage",
            "spec",
            "specification"
        };

        foreach (var keyword in productKeywords)
        {
            if (text.Contains(keyword))
            {
                return Task.FromResult("CALL_SEARCH_PRODUCT:" + message);
            }
        }

        return Task.FromResult("NO_TOOL");
    }
}