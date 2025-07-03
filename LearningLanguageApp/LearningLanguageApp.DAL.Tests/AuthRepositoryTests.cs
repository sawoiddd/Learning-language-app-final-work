using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.DAL.Tests;

public class AuthRepositoryTests
{
    private AuthRepository _repository;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public AuthRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LearningLanguageAppDataContext(options);
        _repository = new AuthRepository(_context, _logger);
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddUser()
    {
        var user = new User
        {
            Login = "test",
            Password = "123",
            FirstName = "TestFirst",
            LastName = "TestLast",
            BirthdayDate = DateTime.UtcNow,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        };
        var result = await _repository.AddUserAsync(user, CancellationToken.None);
        Assert.NotNull(await _context.Users.FirstOrDefaultAsync(u => u.Login == "test"));
    }

    [Fact]
    public async Task GetUserByLoginAsync_ShouldReturnUser_IfExists()
    {
        _context.Users.Add(new User
        {
            Login = "sasha",
            Password = "123",
            FirstName = "TestFirst",
            LastName = "TestLast",
            BirthdayDate = DateTime.UtcNow,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        var user = await _repository.GetUserByLoginAsync("sasha", CancellationToken.None);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task IsLoginUniqueAsync_ShouldReturnFalse_IfExists()
    {
        _context.Users.Add(new User
        {
            Login = "admin",
            Password = "password123",
            FirstName = "AdminFirst",
            LastName = "AdminLast",
            BirthdayDate = DateTime.UtcNow,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        var isUnique = await _repository.IsLoginUniqueAsync("admin", CancellationToken.None);
        Assert.False(isUnique);
    }
}
