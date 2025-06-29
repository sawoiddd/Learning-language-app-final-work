using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IDictionaryService
{
    Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken);
    Task<IEnumerable<Dictionary>> GetUserDictionariesAsync(int userId, CancellationToken cancellationToken);
    Task<Dictionary> CreateDictionaryAsync(Dictionary dictionary, int userId, CancellationToken cancellationToken);
    Task<Dictionary> UpdateDictionaryAsync(Dictionary dictionary, CancellationToken cancellationToken);
    Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken);
}
