using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IGameRepository
{
    Task<IList<Word>> GetRandomWordsByDictionaryAsync(int dictionaryId, int count, CancellationToken cancellationToken);
}
