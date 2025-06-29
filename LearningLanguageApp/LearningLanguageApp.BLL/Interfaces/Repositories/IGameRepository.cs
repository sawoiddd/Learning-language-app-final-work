using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IGameRepository
{
    Task<IList<Word>> GetRandomWordsAsync(int userId, CancellationToken cancellationToken);
}
