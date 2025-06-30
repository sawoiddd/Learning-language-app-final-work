namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IGoogleTranslateRepository
{
    Task<string> GetWordTranslateAsync(string originalWord, string originalLanguage, string targetLanguage, CancellationToken cancellationToken);
}
