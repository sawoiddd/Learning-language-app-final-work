using System.Text.Json;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using Serilog;

namespace LearningLanguageApp.DAL.Repositories;

public class GoogleTranslateRepository : IGoogleTranslateRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    public GoogleTranslateRepository(HttpClient httpClient,ILogger logger)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<string> GetWordTranslateAsync(string originalWord, string originalLanguage, string targetLanguage, CancellationToken cancellationToken)
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={originalLanguage}&tl={targetLanguage}&dt=t&dt=bd&q={Uri.EscapeDataString(originalWord)}";

        var response = await _httpClient.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"Failed to translate word '{originalWord}' from '{originalLanguage}' to '{targetLanguage}'. Status code: {response.StatusCode}");
            throw new Exception("Translation failed");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        using var jsonDoc = JsonDocument.Parse(jsonResponse);
        var root = jsonDoc.RootElement;

        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0 &&
        root[0].ValueKind == JsonValueKind.Array && root[0].GetArrayLength() > 0 &&
        root[0][0].ValueKind == JsonValueKind.Array && root[0][0].GetArrayLength() > 0)
        {
            _logger.Information($"Successfully translated word '{originalWord}' from '{originalLanguage}' to '{targetLanguage}'");
            return root[0][0][0].GetString() ?? string.Empty;
        }

        return string.Empty;
    }
}
