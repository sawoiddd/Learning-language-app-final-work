using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.Services;

public class GameService : IGameSerivce
{

    private readonly IGameRepository _gameRepository;

    private IList<Word> _currentWords;


    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }


    public async Task<IList<Word>> StartGameAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            throw new Exception("Invalid dictionary id");
        }

        //10 words just for now later (if we have time we will give user choice)
        _currentWords = await _gameRepository.GetRandomWordsByDictionaryAsync(dictionaryId, 10, cancellationToken);
        return _currentWords;
    }

    public GameResult CheckAnswers(IDictionary<int, string> userAnswers, GameMode mode)
    {
        int correct = 0;

        foreach (var answer in userAnswers)
        {
            var word = _currentWords.FirstOrDefault(w => w.Id == answer.Key);
            if (word == null) continue;

            string expected = mode == GameMode.OriginalToTranslation ? word.Translation : word.OriginalWord;

            if (string.Equals(expected, answer.Value, StringComparison.OrdinalIgnoreCase))
            {
                correct++;
            }
        }

        return new GameResult
        {
            TotalWords = _currentWords.Count,
            CorrectAnswers = correct
        };
    }
}