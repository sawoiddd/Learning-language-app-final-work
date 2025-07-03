namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IGoogleTranslateService
{
    Task<string> GetTranslateAsync(string word, string originalLanguage, string targetLanguage, CancellationToken cancellationToken);
}
