using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.DAL;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LearningLanguageApp.Services;

public class Dependency
{
    private static IConfiguration _configuration;

    public Dependency(string fileName)
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(fileName, optional: false)
            .Build();
    }

    private static HttpClient GetHttpClient()
    {
        return new HttpClient();
    }
    private static LearningLanguageAppDataContext GetContext()
    {
        var path = _configuration["DbSettings:Path"];
        return new LearningLanguageAppDataContext(path);
    }

    private static IAuthRepository GetAuthRepository()
    {
        return new AuthRepository(GetContext(), LoggerService.GetLogger());
    }
    public static IAuthSerivce GetAuthService()
    {
        return new AuthService(GetAuthRepository(), LoggerService.GetLogger());
    }

    private static IDictionaryRepository GetDictionaryRepository()
    {
        return new DictionaryRepository(GetContext(), LoggerService.GetLogger());
    }
    public static IDictionaryService GetDictionaryService()
    {
        return new DictionaryService(GetDictionaryRepository(), LoggerService.GetLogger());
    }

    private static IWordRepository GetWordRepository()
    {
        return new WordRepository(GetContext(), LoggerService.GetLogger());
    }
    public static IWordService GetWordService()
    {
        return new WordService(GetWordRepository(), LoggerService.GetLogger());
    }

    private static IGoogleTranslateRepository GetTranslationRepository()
    {
        return new GoogleTranslateRepository(GetHttpClient(), LoggerService.GetLogger());
    }
    public static IGoogleTranslateService GetTranslationService()
    {
        return new GoogleTranslatorService(GetTranslationRepository(), LoggerService.GetLogger());
    }

    private static IGameRepository GetGameRepository()
    {
        return new GameRepository(GetContext(), LoggerService.GetLogger());
    }
    public static IGameSerivce GetGameSerivce()
    {
        return new GameService(GetGameRepository(), LoggerService.GetLogger());
    }
}

