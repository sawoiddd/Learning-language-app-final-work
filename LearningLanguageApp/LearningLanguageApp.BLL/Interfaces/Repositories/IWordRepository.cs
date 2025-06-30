using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IWordRepository
{
    Task<Word> AddWordAsync(int dictionaryId, AddWordDto word, CancellationToken cancellationToken);
    Task<Word> UpdateWordAsync(Word word, CancellationToken cancellationToken);
    Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken);
    Task<Word> LearnWordAsync(int wordId, CancellationToken cancellationToken);
    Task<IEnumerable<Word>> GetWordsByDictionaryAsync(int dictionaryId, CancellationToken cancellationToken);
}
