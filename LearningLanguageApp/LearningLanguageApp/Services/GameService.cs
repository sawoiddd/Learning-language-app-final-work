using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using Serilog;

namespace LearningLanguageApp.Services;

public class GameService : IGameSerivce
{
    private readonly IGameRepository _gameRepository;
    private IList<Word> _currentWords;
    private readonly ILogger _logger;
    private const int CountOfWords = 10;

    public GameService(IGameRepository gameRepository, ILogger logger)
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }

    public async Task<IList<Word>> StartGameAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            _logger.Error($"Invalid dictionary id: {dictionaryId}");
            throw new Exception("Invalid dictionary id");
        }
        
        _logger.Information($"Starting game with dictionary id: {dictionaryId}");
        _currentWords = await _gameRepository.GetRandomWordsByDictionaryAsync(dictionaryId, CountOfWords, cancellationToken);
        return _currentWords;
    }

    public GameResult CheckAnswers(IDictionary<int, string> userAnswers, GameMode mode)
    {
        int correct = 0;

        foreach (var answer in userAnswers)
        {
            var word = _currentWords.FirstOrDefault(x => x.Id.Equals(answer.Key));
            if (word == null)
            {
                continue;
            }

            string expected = mode == GameMode.OriginalToTranslation ? word.Translation : word.OriginalWord;

            if (string.Equals(expected, answer.Value, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Information($"Correct answer for word ID {word.Id}: {expected}");
                correct++;
            }
        }

        _logger.Information($"Game completed. Total words: {_currentWords.Count}, Correct answers: {correct}");
        return new GameResult
        {
            TotalWords = _currentWords.Count,
            CorrectAnswers = correct
        };
    }
}