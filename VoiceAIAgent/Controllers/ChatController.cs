using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("ask")]
    public IActionResult Ask([FromBody] ChatRequest request)
    {
        var chunks = _ragService.Retrieve(request.Message);

        if (!chunks.Any() || chunks.First().Score == 0)
        {
            return Ok(new
            {
                answer = "Sorry, I couldn't find anything related to your question."
            });
        }

        var context = string.Join(
            "\n\n-----------------\n\n",
            chunks.Select(x => x.Content));

        string response =
            $"""
            Based on the retrieved knowledge base information:

            {context}

            This information was retrieved using the RAG engine.

            Would you like more information about pricing, shipping or specifications?
            """;

        return Ok(new
        {
            answer = response
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