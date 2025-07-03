using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using Serilog;
using System;

namespace LearningLanguageApp.Services;

public class GameService : IGameSerivce
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger _logger;
    private const int CountOfWords = 10;

    public GameService(IGameRepository gameRepository, ILogger logger)
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }

    public async Task<List<Word>> StartGameAsync(int dictionaryId, GameMode gameMode, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            _logger.Error($"Invalid dictionary id: {dictionaryId}");
            throw new Exception("Invalid dictionary id");
        }
        
        var words = await _gameRepository.GetRandomWordsByDictionaryAsync(dictionaryId, CountOfWords, cancellationToken);
        
        if (words == null)
        {
            _logger.Error($"Failed found words in dictionary {dictionaryId}");
            throw new KeyNotFoundException($"No words found in dictionary {dictionaryId}");
        }

        _logger.Information($"Starting game with dictionary id: {dictionaryId}");
        return words;
    }

    public GameResult CheckAnswers(IDictionary<int, string> userAnswers, GameMode gameMode, List<Word> currentWords)
    {
        int correct = 0;

        foreach (var answer in userAnswers)
        {
            int index = answer.Key - 1;

            if (index < 0 || index >= currentWords.Count)
                continue;

            var word = currentWords[index];

            string expected = gameMode == GameMode.OriginalToTranslation ? word.Translation : word.OriginalWord;

            if (string.Equals(expected, answer.Value, StringComparison.OrdinalIgnoreCase))
            {
                _logger.Information($"Correct answer for word ID {word.Id}: {expected}");
                correct++;
            }
        }

        _logger.Information($"Game completed. Total words: {currentWords.Count}, Correct answers: {correct}");
        return new GameResult
        {
            TotalWords = currentWords.Count,
            CorrectAnswers = correct
        };
    }
}