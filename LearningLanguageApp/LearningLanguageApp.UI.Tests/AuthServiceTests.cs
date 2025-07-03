using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.DAL;
using LearningLanguageApp.DAL.Repositories;
using LearningLanguageApp.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.UI.Tests;

public class AuthServiceTests
{
    private readonly IAuthRepository _authRepository;
    private readonly IAuthSerivce _authSerivce;
    private LearningLanguageAppDataContext _context;
    private ILogger _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<LearningLanguageAppDataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LearningLanguageAppDataContext(options);
        _authRepository = new AuthRepository(_context, _logger);
        _authSerivce = new AuthService(_authRepository,_logger);
    }

    [Fact]
    public async Task RegisterAsync_CorrectRegister()
    {
        var userdto = new RegisterUserDto()
        {
            FirstName = "TestFirst",
            LastName = "TestLast",
            Login = "TestLogin",
            Password = "TestPassword",
            BirthdayDate = DateTime.UtcNow
        };
        var result = _authSerivce.RegisterAsync(userdto, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task LoginAsync_CorrectLogin()
    {
        var userDto = new LoginUserDto()
        {
            Login = "TestLogin",
            Password = "TestPassword"
        };
        var result = _authSerivce.LoginAsync(userDto, CancellationToken.None);
        Assert.NotNull(result);
    }

}
