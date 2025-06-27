using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Models;
using Serilog.Core;

namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IWordRepository
{
    Task<Word> AddWordAsync(Logger logger,AddWordDto word);
    Task<Word> UpdateWordAsync(Logger logger,Word word);
    Task<string> DeleteWordAsync(Logger logger,int wordId);
    Task<string> LearnWordAsync(Logger logger,int wordId);
    Task<string> GetTranslateAsync(Logger logger,string word);
}
