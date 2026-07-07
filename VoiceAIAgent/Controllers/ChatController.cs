using Microsoft.AspNetCore.Mvc;
using VoiceAIAgent.Data;
using VoiceAIAgent.Models;
using VoiceAIAgent.Services;

namespace VoiceAIAgent.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly AIPipelineService _pipeline;

    public ChatController(AIPipelineService pipeline)
    {
        _pipeline = pipeline;
    }


    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequest request)
    {
        var answer = await _pipeline.Process(request.Message);

        return Ok(new
        {
            answer
        });
    }

    [HttpGet("stream")]
    public async Task Stream([FromQuery] string message)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("Connection", "keep-alive");

        await foreach (var chunk in _pipeline.ProcessStream(message))
        {
            await Response.WriteAsync($"data: {chunk}\n\n");
            await Response.Body.FlushAsync();
        }

        await Response.WriteAsync("data: [DONE]\n\n");
        await Response.Body.FlushAsync();
    }

}