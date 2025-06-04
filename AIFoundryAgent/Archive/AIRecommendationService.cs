using System.Text.Json;
using System.Text;

public class AIRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly DatabricksRepository _databricksRepo;
    private readonly IConfiguration _config;

    public AIRecommendationService(HttpClient httpClient, DatabricksRepository databricksRepo, IConfiguration config)
    {
        _httpClient = httpClient;
        _databricksRepo = databricksRepo;
        _config = config;
    }

    public async Task<string> GetRecommendationAsync(object usageData, string userId, string appId, string access_id)
{
    var payload = new
    {
        input = new
        {
            userId,
            appId,
            access_id,
            accessStats = usageData
        }
    };

        var endpoint = _config["AzureAI:Endpoint"];
        var apiKey = _config["AzureAI:ApiKey"];

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AgentResponse>(json);

        return result?.Result?.Output ?? "No recommendation available.";

    }
}
