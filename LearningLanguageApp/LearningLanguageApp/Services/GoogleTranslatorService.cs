using LearningLanguageApp.BLL.Interfaces.Repositories;

namespace LearningLanguageApp.Services;

public class GoogleTranslatorService : IGoogleTranslateRepository
{
    public Task<string> GetWordTranslateAsync(string originalWord, string originalLanguage, string targetLanguage,
        CancellationToken cancellationToken)
    {
            
    }
}