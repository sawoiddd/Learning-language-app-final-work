using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL;
using LearningLanguageApp.DAL.Repositories;
using LearningLanguageApp.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;

namespace LearningLanguageApp.UI.Tests;

public class GameServiceTests
{
    private IGameSerivce _service;
    private IGameRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public GameServiceTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext(options);
        _repository = new GameRepository(_context, _logger);
        _service = new GameService(_repository, _logger);
    }

    [Fact]
    public async Task StartGameAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var dictionaryId = 1;
        _context.Words.AddRange(new[]
       {
            new Word { Id = 1, OriginalWord = "cat", Translation = "кіт", DictionaryID = dictionaryId },
            new Word { Id = 2, OriginalWord = "dog", Translation = "собака", DictionaryID = dictionaryId }
        });
        await _context.SaveChangesAsync();

        var result = await _service.StartGameAsync(dictionaryId, GameMode.OriginalToTranslation, CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CheckAnswers_WithCorrectAnswers_ReturnsCorrectCount()
    {
        var dictionaryId = 1;

        _context.Words.AddRange(new[]
        {
            new Word { Id = 1, OriginalWord = "cat", Translation = "кіт", DictionaryID = dictionaryId },
            new Word { Id = 2, OriginalWord = "dog", Translation = "собака", DictionaryID = dictionaryId }
        });
        await _context.SaveChangesAsync();

        await _service.StartGameAsync(dictionaryId, GameMode.OriginalToTranslation, CancellationToken.None);

        var userAnswers = new Dictionary<int, string>
        {
            { 1, "кіт" },
            { 2, "собака" }
        };

        var result = _service.CheckAnswers(userAnswers);

        Assert.NotNull(result);
    }
}
