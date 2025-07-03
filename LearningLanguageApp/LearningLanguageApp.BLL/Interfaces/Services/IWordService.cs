using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IWordService
{
    Task<Word> AddWordAsync(int dictionaryId, AddWordDto dto, CancellationToken cancellationToken);
    Task<Word> UpdateWordAsync(UpdateWordDto word, CancellationToken cancellationToken);
    Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken);
    Task<Word> MarkAsLearnedAsync(int wordId, CancellationToken cancellationToken);
    Task<IEnumerable<Word>> GetWordsAsync(int dictionaryId, CancellationToken cancellationToken);

}
