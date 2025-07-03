using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.DAL.Repositories;
using LearningLanguageApp.Services;
using Serilog;

namespace LearningLanguageApp.UI.Tests;

public class GoogleTranslateServiceTests
{
    private IGoogleTranslateService _service;
    private IGoogleTranslateRepository _repository;
    private readonly HttpClient _httpClient;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public GoogleTranslateServiceTests()
    {
        _httpClient = new HttpClient();
        _repository = new GoogleTranslateRepository(_httpClient, _logger);
        _service = new GoogleTranslatorService(_repository, _logger);
    }

    [Fact]
    public async Task GetWordTranslateAsync_ShouldReturnTranslation()
    {
        var translation = await _service.GetTranslateAsync("hello", "en", "uk", CancellationToken.None);
        Assert.False(string.IsNullOrWhiteSpace(translation));
    }
}
