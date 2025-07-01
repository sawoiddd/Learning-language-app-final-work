using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using Serilog;

namespace LearningLanguageApp.Services;

public class AuthService : IAuthSerivce
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger _logger;

    public AuthService(IAuthRepository iAuthRepository,  ILogger logger)
    {
        _authRepository = iAuthRepository;
        _logger = logger;
    }

    public async Task<User> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken)
    {
        if (!await _authRepository.IsLoginUniqueAsync(dto.Login, cancellationToken))
        {
            _logger.Error($"Register attempt with non-unique login: {dto.Login}");
            throw new InvalidOperationException("Login is already taken.");
        }

        var user = new User
        {
            Login = dto.Login,
            BirthdayDate = dto.BirthdayDate,
            CreateAt = DateTime.UtcNow,
            UpdateAt = DateTime.UtcNow,
            Dictionaries = new List<Dictionary>(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        return await _authRepository.AddUserAsync(user, cancellationToken);
    }

    public async Task<User> LoginAsync(LoginUserDto dto, CancellationToken cancellationToken)
    {
        var user = await _authRepository.GetUserByLoginAsync(dto.Login, cancellationToken);
        if (user == null)
        {
            _logger.Error($"Login failed. User not found: {dto.Login}");
            throw new KeyNotFoundException("Invalid login or password.");
        }
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            _logger.Error($"Login failed. Incorrect password for user: {dto.Login}");
            throw new UnauthorizedAccessException("Invalid login or password.");
        }

        _logger.Information($"User logged in: {dto.Login}");
        return user;
    }
}