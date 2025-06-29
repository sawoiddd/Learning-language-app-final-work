using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IDictionaryRepository
{
    Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken);
    Task<IEnumerable<Dictionary>> GetByUserDictionaryIdAsync(int userId, CancellationToken cancellationToken);
    Task<Dictionary> AddDictionaryAsync(AddDictionaryDto dictionary, CancellationToken cancellationToken);
    Task<Dictionary> UpdateDictionaryAsync(Dictionary dictionary, CancellationToken cancellationToken);
    Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken);
}
