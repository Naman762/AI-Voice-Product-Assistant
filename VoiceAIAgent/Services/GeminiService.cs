using System.Text;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace VoiceAIAgent.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]!;
    }

    public async Task<string> GetResponse(string prompt)
    {
        try
        {
            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            var body = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
            };

            var json = JsonSerializer.Serialize(body);

            var response = await _httpClient.PostAsync(
                url,
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                switch ((int)response.StatusCode)
                {
                    case 429:
                        return "I'm receiving a lot of requests right now. Please wait a few moments and try again.";

                    case 503:
                        return "My AI service is temporarily unavailable. Please try again shortly.";

                    case 401:
                    case 403:
                        return "My AI service isn't configured correctly at the moment.";

                    case 500:
                    case 502:
                    case 504:
                        return "Something went wrong while generating my response. Please try again.";

                    default:
                        return "I'm having trouble answering right now. Please try again in a moment.";
                }
            }

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";

            text = text
                .Replace("**", "")
                .Replace("*", "")
                .Replace("#", "")
                .Replace("`", "")
                .Replace("_", "");

            return text;
        }
        catch (TaskCanceledException)
        {
            return "The request took too long. Please try again.";
        }
        catch (HttpRequestException)
        {
            return "I couldn't connect to my AI service. Please check your internet connection and try again.";
        }
        catch (Exception)
        {
            return "Something unexpected happened. Please try again.";
        }
    }
    public async Task<string> GetRawResponse(string prompt)
    {
        return await GetResponse(prompt);
    }

    public async IAsyncEnumerable<string> StreamResponse(
    string prompt,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:streamGenerateContent?alt=sse&key={_apiKey}";

        var body = new
        {
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = prompt
                    }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);

        request.Content =
            new StringContent(json, Encoding.UTF8, "application/json");

        using var response =
            await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        await using var stream =
            await response.Content.ReadAsStreamAsync(cancellationToken);

        using var reader = new StreamReader(stream);
        string previous = "";

        while (true)
        {
            var line = await reader.ReadLineAsync();

            if (line == null)
                break;
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (!line.StartsWith("data:"))
                continue;

            var jsonLine = line.Substring(5).Trim();

            JsonDocument? doc = null;

            try
            {
                doc = JsonDocument.Parse(jsonLine);
            }
            catch
            {
                continue;
            }

            var candidates = doc.RootElement.GetProperty("candidates");

            var text =
     candidates[0]
         .GetProperty("content")
         .GetProperty("parts")[0]
         .GetProperty("text")
         .GetString();

            if (string.IsNullOrWhiteSpace(text))
                continue;

            if (text.StartsWith(previous))
            {
                var delta = text.Substring(previous.Length);

                if (!string.IsNullOrEmpty(delta))
                    yield return delta;
            }
            else
            {
                yield return text;
            }

            previous = text;
        }
    }
}

