using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    [Fact] //(Skip = "Integration test hitting live API")
    public async Task GetWordTranslateAsync_ShouldReturnTranslation()
    {
        var translation = await _repository.GetWordTranslateAsync("hello", "en", "uk", CancellationToken.None);
        Assert.False(string.IsNullOrWhiteSpace(translation));
    }
}
