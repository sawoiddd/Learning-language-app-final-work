using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IGameRepository
{
    Task<List<Word>> GetRandomWordsByDictionaryAsync(int dictionaryId, int count, CancellationToken cancellationToken);
}
