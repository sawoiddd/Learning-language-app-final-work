using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Xunit;
namespace LearningLanguageApp.DAL.Tests;

public class DictionaryRepositoryTests
{
    private DictionaryRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public DictionaryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext("FakeConnection");
        _context = new LearningLanguageAppDataContext(options);

        _repository = new DictionaryRepository(_context, _logger);
    }

    [Fact]
    public async Task AddDictionaryAsync_ShouldAdd()
    {
        var dict = new Dictionary
        {
            UserId = 1,
            SourceLanguage = "en",
            TargetLanguage = "uk",
            Words = new List<Word>()
        };
        var result = await _repository.AddDictionaryAsync(dict, CancellationToken.None);
        Assert.True(_context.Dictionaries.Any(d => d.Id == result.Id));
    }


    [Fact]
    public async Task UpdateDictionaryAsync_ShouldUpdateLanguages()
    {
        var dictionary = new Dictionary
        {
            SourceLanguage = "en",
            TargetLanguage = "uk",
            UserId = 1
        };

        _context.Dictionaries.Add(dictionary);
        await _context.SaveChangesAsync();

        dictionary.SourceLanguage = "de";
        dictionary.TargetLanguage = "fr";

        var updated = await _repository.UpdateDictionaryAsync(dictionary, CancellationToken.None);

        Assert.Equal("de", updated.SourceLanguage);
        Assert.Equal("fr", updated.TargetLanguage);
    }


    [Fact]
    public async Task DeleteDictionaryAsync_ShouldRemove()
    {
        var dict = new Dictionary { SourceLanguage = "en", TargetLanguage = "uk", UserId = 1 };
        _context.Dictionaries.Add(dict);
        await _context.SaveChangesAsync();

        var deleted = await _repository.DeleteDictionaryAsync(dict.Id, CancellationToken.None);
        Assert.Null(await _context.Dictionaries.FindAsync(dict.Id));
    }

    [Fact]
    public async Task GetByUserDictionaryIdAsync_ShouldReturnAll()
    {
        _context.Dictionaries.AddRange(
            new Dictionary { UserId = 1, SourceLanguage = "en", TargetLanguage = "uk" },
            new Dictionary { UserId = 1, SourceLanguage = "en", TargetLanguage = "uk" }
        );
        await _context.SaveChangesAsync();

        var list = await _repository.GetByUserDictionaryIdAsync(1, CancellationToken.None);
        Assert.Equal(2, list.Count());
    }


    [Fact]
    public async Task GetDictionaryByIdAsync_ShouldReturnDictionaryWithWords()
    {
        var dictionary = new Dictionary { SourceLanguage = "en", TargetLanguage = "uk", UserId = 1 };
        var word = new Word { OriginalWord = "test", Translation = "тест", Dictionary = dictionary };

        _context.Dictionaries.Add(dictionary);
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        var result = await _repository.GetDictionaryByIdAsync(dictionary.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Words);
    }
}
