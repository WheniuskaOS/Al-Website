using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FsHttp;

public class OpenAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetResponse(string userMessage)
    {
        var requestBody = new
        {
            model = "gpt-4.1-mini",
            input = userMessage
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");
        
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
        
        var response = await _httpClient.PostAsync(
            "https://api.openai.com/v1/responses",
            jsonContent);

        var responseString = await response.Content.ReadAsStringAsync();

        var json = JsonDocument.Parse(responseString);

        return json.RootElement
                   .GetProperty("output")[0]
                   .GetProperty("content")[0]
                   .GetProperty("text")
                   .GetString()!;
    }
}