using LearningLanguageApp.DAL.Repositories;
using Serilog;

namespace LearningLanguageApp.DAL.Tests;

public class GoogleTranslateRepositoryTests
{
    private GoogleTranslateRepository _repository;
    private readonly HttpClient _httpClient;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public GoogleTranslateRepositoryTests()
    {
        _httpClient = new HttpClient();
        _repository = new GoogleTranslateRepository(_httpClient, _logger);
    }

    [Fact]
    public async Task GetWordTranslateAsync_ShouldReturnTranslation()
    {
        var translation = await _repository.GetWordTranslateAsync("hello", "en", "uk", CancellationToken.None);
        Assert.False(string.IsNullOrWhiteSpace(translation));
    }
}
