using Discord;
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

public class DictionaryServiceTests
{
    private IDictionaryRepository _repository;
    private IDictionaryService _service;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public DictionaryServiceTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _context = new LearningLanguageAppDataContext(options);
        _repository = new DictionaryRepository(_context, _logger);
        _service = new DictionaryService(_repository, _logger);
    }

    [Fact]
    public async Task GetDictionaryByIdAsync_CorrectId()
    {
        var dictionary = new Dictionary
        {
            Id = 1,
            SourceLanguage = "test",
            TargetLanguage = "test",
            UserId = 1,
            Words = new List<Word>()
        };
        _context.Dictionaries.Add(dictionary);
        await _context.SaveChangesAsync();

        var dictionaryId = 1;

        var result = await _service.GetDictionaryByIdAsync(dictionaryId, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserDictionariesAsync_CorrectId()
    {
        var dictionary = new Dictionary
        {
            Id = 1,
            SourceLanguage = "test",
            TargetLanguage = "test",
            UserId = 1,
            Words = new List<Word>()
        };
        var user = new User
        {
            Id = 1,
            FirstName = "test",
            LastName = "test",
            BirthdayDate = DateTime.Now,
            Dictionaries = new List<Dictionary> { dictionary },
            Login = "test",
            Password = "test",
            CreateAt = DateTime.Now,
            UpdateAt = DateTime.Now
        };

        _context.Dictionaries.Add(dictionary);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userId = 1;

        var result = await _service.GetUserDictionariesAsync(userId, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateDictionaryAsync_CorrectDictionary()
    {
        var userId = 1;
        var dictionary = new AddDictionaryDto
        {
            SourceLanguage = "test",
            TargetLanguage = "test",
            UserId = userId
        };

        var result = await _service.CreateDictionaryAsync(dictionary, userId, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateDictionaryAsync_CorrectDictionary()
    {
        var dictionaryId = 1;
        var dictionary = new Dictionary
        {
            Id = dictionaryId,
            SourceLanguage = "test",
            TargetLanguage = "test",
            UserId = 1,
            Words = new List<Word>()
        };

        _context.Dictionaries.Add(dictionary);
        await _context.SaveChangesAsync();

        var dictionaryUpdate = new UpdateDictionaryDto
        {
            Id = dictionaryId,
            SourceLanguage = "test",
            TargetLanguage = "test"
        };

        var result = await _service.UpdateDictionaryAsync(dictionaryUpdate, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteDictionaryAsync_CorrectId()
    {
        var dictionaryId = 1;
        var dictionary = new Dictionary
        {
            Id = dictionaryId,
            SourceLanguage = "test",
            TargetLanguage = "test",
            UserId = 1,
            Words = new List<Word>()
        };

        _context.Dictionaries.Add(dictionary);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteDictionaryAsync(dictionaryId, CancellationToken.None);
        Assert.NotNull(result);
    }
}
