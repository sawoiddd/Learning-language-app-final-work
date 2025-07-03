using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.BLL.Interfaces.Services;

public interface IGameSerivce
{
    Task<List<Word>> StartGameAsync(int dictionaryId, GameMode gameMode, CancellationToken cancellationToken);
    GameResult CheckAnswers(IDictionary<int, string> userAnswers, GameMode mode, List<Word> words);
}
