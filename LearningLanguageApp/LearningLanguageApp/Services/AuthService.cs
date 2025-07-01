using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Serilog;

namespace LearningLanguageApp.Services;

public class AuthService : IAuthRepository
{
    private readonly IAuthRepository _iAuthRepository;
    private readonly ILogger _logger;

    public AuthService(IAuthRepository iAuthRepository,  ILogger logger)
    {
        _iAuthRepository = iAuthRepository;
        _logger = logger;
    }
    
    public Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new Exception ("Login cannot be empty");
        }
        
        return  _iAuthRepository.GetUserByLoginAsync(login, cancellationToken);
    }

    public Task<bool> IsLoginUniqueAsync(string login, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new Exception ("Login cannot be empty");
        }
        
        return _iAuthRepository.IsLoginUniqueAsync(login, cancellationToken);
    }

    public Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(user.Login))
        {
            throw new Exception ("Login cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new Exception ("Password cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(user.FirstName))
        {
            throw new Exception ("First name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(user.LastName))
        {
            throw new Exception ("Last name cannot be empty");
        }

        if (user.BirthdayDate >= DateTime.Today)
        {
            throw new Exception ("Birthday date is invalid");
        }

        var userNew = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthdayDate = user.BirthdayDate,
            Login = user.Login,
            Password = user.Password,
            CreateAt = DateTime.Now,
            UpdateAt = DateTime.Now
        };
        
        return _iAuthRepository.AddUserAsync(userNew, cancellationToken);

    }
}