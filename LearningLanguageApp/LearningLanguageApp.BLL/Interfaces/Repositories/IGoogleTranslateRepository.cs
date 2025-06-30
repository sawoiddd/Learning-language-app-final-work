namespace LearningLanguageApp.BLL.Interfaces.Repositories;

public interface IGoogleTranslateRepository
{
    Task<string> GetWordTranslateAsync(string originalWord, CancellationToken cancellationToken);
}
