using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IWordService
{
    Task<Word> AddWordAsync(Word word);
    Task<Word> UpdateWordAsync(Word word);
    Task<string> DeleteWordAsync(int wordId);
    Task<string> LearnWordAsync(int wordId);
}
