using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IAuthSerivce
{
    Task<User> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken);
    Task<User> LoginAsync(LoginUserDto dto, CancellationToken cancellationToken);
}
