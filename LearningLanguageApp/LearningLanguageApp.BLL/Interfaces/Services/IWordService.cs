
using LearningLanguageApp.BLL.Models;
using Serilog.Core;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IWordService
{
    Task<Word> AddWordAsync(Logger logger, Word word);
    Task<Word> UpdateWordAsync(Logger logger, Word word);
    Task<string> DeleteWordAsync(Logger logger, int wordId);
    Task<string> LearnWordAsync(Logger logger, int wordId);
}
