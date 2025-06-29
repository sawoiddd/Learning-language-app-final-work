using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IWordRepository
{
    Task<Word> AddWordAsync(AddWordDto word);
    Task<Word> UpdateWordAsync(Word word);
    Task<string> DeleteWordAsync(int wordId);
    Task<string> LearnWordAsync(int wordId);
    Task<string> GetTranslateAsync(string word);
}
