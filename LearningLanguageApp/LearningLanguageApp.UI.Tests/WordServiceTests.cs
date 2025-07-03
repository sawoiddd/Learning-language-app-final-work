using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL;
using LearningLanguageApp.DAL.Repositories;
using LearningLanguageApp.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.UI.Tests;

public class WordServiceTests
{
    private IWordService _service;
    private IWordRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public WordServiceTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext(options);
        _repository = new WordRepository(_context, _logger);
        _service = new WordService(_repository, _logger);
    }

    [Fact]
    public async Task AddWordAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var word = new AddWordDto { OriginalWord = "cat", Translation = "кіт" };

        var result = await _service.AddWordAsync(1, word, CancellationToken.None);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateWordAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var word = new Word
        {
            Id = 1,
            OriginalWord = "cat",
            Translation = "кіт",
            Type = BLL.Enums.WordTypeEnum.Noun,
            Level = BLL.Enums.WordLevelEnum.A1,
            DictionaryID = 1
        };

        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var updateWord = new UpdateWordDto
        {
            Id = 1,
            OriginalWord = "dog",
            Translation = "пес",
            Type = BLL.Enums.WordTypeEnum.Noun,
            Level = BLL.Enums.WordLevelEnum.A1
        };   

        var updated = await _service.UpdateWordAsync(updateWord, CancellationToken.None);

        Assert.NotNull(updated);
    }

    [Fact]
    public async Task DeleteWordAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var dicId = 1;
        var word = new Word { OriginalWord = "delete", Translation = "видалити", DictionaryID = dicId };
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var deleted = await _service.DeleteWordAsync(word.Id, CancellationToken.None);
        Assert.Null(await _context.Words.FindAsync(word.Id));
    }

    [Fact]
    public async Task MarkAsLearnedAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var word = new Word { OriginalWord = "learn", Translation = "вивчати", DictionaryID = 1 };
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var learned = await _service.MarkAsLearnedAsync(word.Id, CancellationToken.None);

        Assert.True(learned.IsLearned);
    }

    [Fact]
    public async Task GetWordsAsync_WithCorrectAnswers_ReturnsNotNullResult()
    {
        var dicId = 10;
        var words = new[]
        {
            new Word { OriginalWord = "one", Translation = "один", DictionaryID = dicId },
            new Word { OriginalWord = "two", Translation = "два", DictionaryID = dicId }
        };

        _context.Words.AddRange(words);
        await _context.SaveChangesAsync();

        var result = await _service.GetWordsAsync(dicId, CancellationToken.None);

        Assert.Equal(2, result.Count());
    }
}
