using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
    Task<bool> IsLoginUniqueAsync(string login, CancellationToken cancellationToken);
    Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
}
