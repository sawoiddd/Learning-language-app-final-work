using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.DAL.Tests;

public class WordRepositoryTests
{
    private WordRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public WordRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext("FakeConnection");
        _context = new LearningLanguageAppDataContext(options);

        _repository = new WordRepository(_context, _logger);
    }

    [Fact]
    public async Task AddWordAsync_ShouldAddWord()
    {
        var word = new Word { OriginalWord = "cat", Translation = "кіт" };
        var result = await _repository.AddWordAsync(1, word, CancellationToken.None);
        Assert.Equal("cat", result.OriginalWord);
    }
    [Fact]
    public async Task UpdateWordAsync_ShouldUpdateWordProperties()
    {
        var word = new Word
        {
            OriginalWord = "cat",
            Translation = "кіт",
            Type = BLL.Enums.WordTypeEnum.Noun,
            Level = BLL.Enums.WordLevelEnum.A1,
            DictionaryID = 1
        };

        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        word.OriginalWord = "dog";
        word.Translation = "пес";
        word.Type = BLL.Enums.WordTypeEnum.Noun;
        word.Level = BLL.Enums.WordLevelEnum.A1;
        word.IsLearned = true;

        var updated = await _repository.UpdateWordAsync(word, CancellationToken.None);

        Assert.Equal("dog", updated.OriginalWord);
        Assert.Equal("пес", updated.Translation);
        Assert.True(updated.IsLearned);
        Assert.Equal(BLL.Enums.WordLevelEnum.A1, updated.Level);
    }
    [Fact]
    public async Task DeleteWordAsync_ShouldDeleteWord()
    {
        var word = new Word { OriginalWord = "delete", Translation = "видалити", DictionaryID = 1 };
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var deleted = await _repository.DeleteWordAsync(word.Id, CancellationToken.None);
        Assert.Null(await _context.Words.FindAsync(word.Id));
    }

    [Fact]
    public async Task LearnWordAsync_ShouldSetLearned()
    {
        var word = new Word { OriginalWord = "learn", Translation = "вивчати", DictionaryID = 1 };
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var learned = await _repository.LearnWordAsync(word.Id, CancellationToken.None);
        Assert.True(learned.IsLearned);
    }


    [Fact]
    public async Task GetWordsByDictionaryAsync_ShouldReturnWords()
    {
        var words = new[]
        {
        new Word { OriginalWord = "one", Translation = "один", DictionaryID = 10 },
        new Word { OriginalWord = "two", Translation = "два", DictionaryID = 10 }
    };

        _context.Words.AddRange(words);
        await _context.SaveChangesAsync();

        var result = await _repository.GetWordsByDictionaryAsync(10, CancellationToken.None);

        Assert.Equal(2, result.Count());
    }
}
