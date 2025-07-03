using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using Serilog;

namespace LearningLanguageApp.Services;

public class GoogleTranslatorService : IGoogleTranslateService
{
    private readonly IGoogleTranslateRepository  _googleTranslateRepository;
    private readonly ILogger _logger;

    public GoogleTranslatorService(IGoogleTranslateRepository googleTranslatorRepository, ILogger logger)
    {
        _logger = logger;
        _googleTranslateRepository = googleTranslatorRepository;
    }

    public async Task<string> GetTranslateAsync(string originalWord, string originalLanguage, string targetLanguage,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(originalWord))
        {
            _logger.Error("The original word cannot be null or empty.");
            throw new Exception("The original word cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(originalLanguage))
        {
            _logger.Error("The original language cannot be null or empty.");
            throw new Exception("The original language cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(targetLanguage))
        {
            _logger.Error("The target language cannot be null or empty.");
            throw new Exception("The target language cannot be null or empty.");
        }
        
        var result = await _googleTranslateRepository.GetWordTranslateAsync(originalWord, originalLanguage, targetLanguage, cancellationToken);

        if (string.IsNullOrEmpty(result))
        {
            _logger.Error($"Translation failed for word '{originalWord}' from '{originalLanguage}' to '{targetLanguage}'");
            throw new Exception("Translation failed");
        }

        _logger.Information($"Translating word '{originalWord}' from '{originalLanguage}' to '{targetLanguage}'");
        return result;
    }
}