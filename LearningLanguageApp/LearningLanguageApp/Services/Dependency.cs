using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.DAL;
using LearningLanguageApp.DAL.Repositories;
using Microsoft.Extensions.Configuration;

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
    private static LearningLanguageAppDataContext GetContext()
    {
        var path = _configuration["DbSettings:Path"];
        return new LearningLanguageAppDataContext(path);
    }

    private static IWordRepository GetWordRepository()
    {
        return new WordRepository(GetContext(), LoggerService.GetLogger());
    }
    public static IWordService GetWordService()
    {
        return new WordService(GetWordRepository(), LoggerService.GetLogger());
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
