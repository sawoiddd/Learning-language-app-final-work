using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.DAL.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly LearningLanguageAppDataContext _context;
    private readonly ILogger _logger;

    public AuthRepository(LearningLanguageAppDataContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information($"User added: {user.Login}");
        return user;
    }

    public Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);

        if (user == null)
        {
            _logger.Error($"User with login {login} not found.");
            throw new KeyNotFoundException($"User with login {login} not found.");
        }

        _logger.Information($"User retrieved: {login}");
        return user;
    }

    public async Task<bool> IsLoginUniqueAsync(string login, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.Login == login, cancellationToken);
    }
}
