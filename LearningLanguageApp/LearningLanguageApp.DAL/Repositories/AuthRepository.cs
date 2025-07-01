
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
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.Information("User added: {Login}", user.Login);

        return user;
    }

    public async Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<bool> IsLoginUniqueAsync(string login, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.Login == login, cancellationToken);
    }
}
