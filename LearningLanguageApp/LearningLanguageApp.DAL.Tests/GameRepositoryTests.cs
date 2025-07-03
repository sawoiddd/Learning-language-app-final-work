using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.DAL.Tests;

public class GameRepositoryTests
{
    private GameRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public GameRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext(options);
        _repository = new GameRepository(_context, _logger);
    }

    [Fact]
    public async Task GetRandomWordsByDictionaryAsync_ShouldReturnRandomWords()
    {
        int dictId = 1;
        for (int i = 0; i < 5; i++)
        {
            _context.Words.Add(new Word
            {
                DictionaryID = dictId,
                OriginalWord = $"word{i}",
                Translation = $"переклад{i}"
            });
        }
        await _context.SaveChangesAsync();

        var result = await _repository.GetRandomWordsByDictionaryAsync(dictId, 3, CancellationToken.None);
        Assert.Equal(3, result.Count);
    }

}
