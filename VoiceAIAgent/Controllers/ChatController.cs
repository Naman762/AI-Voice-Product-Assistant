using Microsoft.AspNetCore.Mvc;
using VoiceAIAgent.Data;
using VoiceAIAgent.Models;
using VoiceAIAgent.Services;

namespace VoiceAIAgent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly RagService _ragService;

    public ChatController(RagService ragService)
    {
        _ragService = ragService;
    }

    private static Product? _lastProduct;

    [HttpPost("ask")]
    public IActionResult Ask([FromBody] ChatRequest request)
    {
        var query = request.Message.ToLower();

        Product? product = null;

        bool followUp =
            query.Contains("price") ||
            query.Contains("shipping") ||
            query.Contains("spec") ||
            query.Contains("guide") ||
            query.Contains("camera") ||
            query.Contains("battery") ||
            query.Contains("cost");

        if (followUp && _lastProduct != null)
        {
            product = _lastProduct;
        }
        else
        {
            var chunks = _ragService.Retrieve(query);

            if (!chunks.Any())
            {
                return Ok(new
                {
                    answer = "Sorry, I couldn't find anything related to your question."
                });
            }

            product = ProductRepository.Products
                .FirstOrDefault(x => x.Name == chunks.First().ProductName);

            _lastProduct = product;
        }

        if (product == null)
        {
            return Ok(new
            {
                answer = "Sorry, I couldn't find the requested product."
            });
        }

        if (query.Contains("price") || query.Contains("cost"))
        {
            return Ok(new
            {
                answer = $"{product.Name} costs ${product.Price}."
            });
        }

        if (query.Contains("shipping"))
        {
            return Ok(new
            {
                answer = "Shipping charges: " +
                         string.Join(", ",
                         product.ShippingPrices.Select(x => $"{x.Key}: ${x.Value}"))
            });
        }

        if (query.Contains("spec"))
        {
            return Ok(new
            {
                answer = $"Specifications:\n{product.Specifications}"
            });
        }

        if (query.Contains("guide"))
        {
            return Ok(new
            {
                answer = product.UserGuide
            });
        }

        if (query.Contains("camera"))
        {
            return Ok(new
            {
                answer = "Galaxy S25 features a 50MP main camera."
            });
        }

        if (query.Contains("battery"))
        {
            return Ok(new
            {
                answer = "Galaxy S25 offers excellent all-day battery life."
            });
        }

        return Ok(new
        {
            answer =
                    $"""
                I found information about {product.Name}.

                Category: {product.Category}

                Price: ${product.Price}

                Description:
                {product.Description}

                What would you like to know?

                • Price
                • Specifications
                • Shipping
                • User Guide
                • Camera
                • Battery
                """
        });
    }
    [HttpGet("stream")]
    public async Task Stream([FromQuery] string message)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");

        var chunks = _ragService.Retrieve(message);

        string answer;

        if (!chunks.Any() || chunks.First().Score == 0)
        {
            answer = "Sorry, I couldn't find anything.";
        }
        else
        {
            var context = string.Join("\n", chunks.Select(x => x.Content));

         answer =
            $"""
        Based on the retrieved knowledge:

        {context}

        Would you like more information?
        """;
        }

        foreach (var word in answer.Split(' '))
        {
            await Response.WriteAsync($"data:{word} \n\n");

            await Response.Body.FlushAsync();

            await Task.Delay(35);
        }

        await Response.WriteAsync("data:[DONE]\n\n");
        await Response.Body.FlushAsync();
    }
}