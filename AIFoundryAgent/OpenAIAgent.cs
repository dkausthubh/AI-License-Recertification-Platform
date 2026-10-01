using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

public class OpenAIAgentService
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;
    private readonly string _deploymentId;
    private readonly string _apiVersion;
    private readonly string _apiKey;

    public OpenAIAgentService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;

        _endpoint = configuration["AzureAI:Endpoint"]
            ?? throw new InvalidOperationException(
                "AzureAI:Endpoint is not configured.");

        _deploymentId = configuration["AzureAI:DeploymentId"]
            ?? throw new InvalidOperationException(
                "AzureAI:DeploymentId is not configured.");

        _apiVersion = configuration["AzureAI:ApiVersion"]
            ?? throw new InvalidOperationException(
                "AzureAI:ApiVersion is not configured.");

        _apiKey = configuration["AzureAI:ApiKey"]
            ?? throw new InvalidOperationException(
                "AzureAI:ApiKey is not configured.");
    }

    public async Task<string> GetAgentResponseAsync(string userMessage)
    {
        var url =
            $"{_endpoint}/openai/deployments/{_deploymentId}/chat/completions" +
            $"?api-version={_apiVersion}";

        var requestBody = new
        {
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = userMessage
                }
            }
        };

        var requestJson = JsonConvert.SerializeObject(requestBody);

        using var content = new StringContent(
            requestJson,
            Encoding.UTF8,
            "application/json");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            url);

        request.Headers.Add("api-key", _apiKey);
        request.Content = content;

        var response = await _httpClient.SendAsync(request);

        var responseJson =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Agent call failed: {response.StatusCode}\n{responseJson}");
        }

        dynamic result =
            JsonConvert.DeserializeObject(responseJson)
            ?? throw new InvalidOperationException(
                "Azure OpenAI returned an empty response.");

        return result.choices[0].message.content;
    }
}