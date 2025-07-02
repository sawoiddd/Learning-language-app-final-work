using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IGameSerivce
{
    Task<IList<Word>> StartGameAsync(int dictionaryId, GameMode mode, CancellationToken cancellationToken);
    GameResult CheckAnswers(IDictionary<int, string> userAnswers, GameMode mode);
}
