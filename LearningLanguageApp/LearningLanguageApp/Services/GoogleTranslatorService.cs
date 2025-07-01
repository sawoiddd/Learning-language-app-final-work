using LearningLanguageApp.BLL.Interfaces.Repositories;
using Serilog;

namespace LearningLanguageApp.Services;

public class GoogleTranslatorService : IGoogleTranslateRepository
{
    private readonly IWordRepository _wordRepository;
    private readonly IGoogleTranslateRepository  _googleTranslateRepository;
    private readonly ILogger _logger;

    public GoogleTranslatorService(IGoogleTranslateRepository googleTranslatorRepository, ILogger logger)
    {
        _logger = logger;
        _googleTranslateRepository = googleTranslatorRepository;
    }
    
    public Task<string> GetWordTranslateAsync(string originalWord, string originalLanguage, string targetLanguage,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(originalWord))
        {
            throw new Exception("The original word cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(originalLanguage))
        {
            throw new Exception("The original language cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(targetLanguage))
        {
            throw new Exception("The target language cannot be null or empty.");
        }
        
        return _googleTranslateRepository.GetWordTranslateAsync(originalWord, originalLanguage, targetLanguage, cancellationToken);
    }
}