using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using Exception = System.Exception;
using Serilog;

namespace LearningLanguageApp.Services;

public class WordService :  IWordService
{
    private readonly IWordRepository _wordRepository;
    private readonly ILogger _logger;

    public WordService(IWordRepository iWordRepository, ILogger logger)
    {
        _logger = logger;
        _wordRepository = iWordRepository;
    }
    
    public Task<Word> AddWordAsync(int dictionaryId, AddWordDto dto, CancellationToken cancellationToken)
    {
        ValidationAddDto(dto);

        var word = new Word()
        {
            OriginalWord = dto.OriginalWord,
            Translation = dto.Translation,
            Type = dto.Type,
            Level = dto.Level,
            IsLearned = false,
            DictionaryID = dictionaryId,
        };

        _logger.Information($"Adding word '{word.OriginalWord}' to dictionary with ID: {dictionaryId}");
        return _wordRepository.AddWordAsync(dictionaryId, word, cancellationToken);
    }
    
    public async Task<Word> UpdateWordAsync(UpdateWordDto word, CancellationToken cancellationToken)
    {
        ValidationUpdDto(word);
        
        var updatedWord = new Word()
        {
            Id = word.Id,
            OriginalWord = word.OriginalWord,
            Translation = word.Translation,
            IsLearned = false,
            Type = word.Type,
            Level = word.Level,
            DictionaryID = -1
        };

        var result = await _wordRepository.UpdateWordAsync(updatedWord,  cancellationToken);

        if (result == null)
        {
            _logger.Error($"Failed to update word with ID: {word.Id}");
            throw new Exception($"Word with ID {word.Id} not found for update");
        }

        _logger.Information($"Updating word with ID: {word.Id}, Original Word: {word.OriginalWord}");
        return result;
    }

    public async Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken)
    {
        if (wordId <= 0)
        {
            _logger.Error($"Word id is invalid: {wordId}");
            throw new Exception("Word id is invalid");
        }

        var result = await _wordRepository.DeleteWordAsync(wordId, cancellationToken);

        if (result == null)
        {
            _logger.Error($"Failed to delete word with ID: {wordId}");
            throw new Exception($"Word with ID {wordId} not found for deletion");
        }

        _logger.Information($"Deleting word with id: {wordId}");
        return result;
    }

    public async Task<Word> MarkAsLearnedAsync(int wordId, CancellationToken cancellationToken)
    {
        if (wordId <= 0)
        {
            _logger.Error($"Word id is invalid: {wordId}");
            throw new Exception("Word id is invalid");
        }
    
        var result = await _wordRepository.LearnWordAsync(wordId, cancellationToken);

        if (result == null)
        {
            _logger.Error($"Failed to mark word with ID: {wordId} as learned");
            throw new Exception($"Word with ID {wordId} not found for learning");
        }

        _logger.Information($"Word with id {wordId} marked as learned");
        return result;
    }

    public async Task<IEnumerable<Word>> GetWordsAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            _logger.Error($"Dictionary ID is invalid: {dictionaryId}");
            throw new Exception("Dictionary ID is invalid");
        }
        
        var result = await _wordRepository.GetWordsByDictionaryAsync(dictionaryId, cancellationToken);

        if (result == null || !result.Any())
        {
            _logger.Error($"Failed to find words in dictionary with ID: {dictionaryId}");
            throw new KeyNotFoundException($"No words found for dictionary with ID: {dictionaryId}");
        }

        _logger.Information($"Retrieving words for dictionary with ID: {dictionaryId}");
        return result;
    }
    
    private void ValidationAddDto(AddWordDto dtoAdd)
    {
        if (string.IsNullOrWhiteSpace(dtoAdd.OriginalWord))
        {
            _logger.Error("Original word could not be empty");
            throw new Exception("Word could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoAdd.Translation))
        {
            _logger.Error("Translation failed");
            throw new Exception("Translation failed");
        }
            
        if (!Enum.IsDefined(typeof(WordTypeEnum), dtoAdd.Type) )
        {
            _logger.Error("This isn't a valid type");
            throw new Exception("This isn't a valid type");
        }
            
        if (!Enum.IsDefined(typeof(WordLevelEnum), dtoAdd.Level))
        {
            _logger.Error("This isn't a valid level");
            throw new Exception("This isn't a valid level");
        }
    }
    private void ValidationUpdDto(UpdateWordDto dtoUpd)
    {
        if (dtoUpd.Id <= 0)
        {
            _logger.Error("Word id is invalid");
            throw new Exception("Word id is invalid");
        }

        if (string.IsNullOrWhiteSpace(dtoUpd.OriginalWord))
        {
            _logger.Error("Word could not be empty");
            throw new Exception("Word could not be empty");
        }

        if (string.IsNullOrWhiteSpace(dtoUpd.Translation))
        {
            _logger.Error("Translation failed");
            throw new Exception("Translation failed");
        } 

        if (!Enum.IsDefined(typeof(WordTypeEnum), dtoUpd.Type) )
        {
            _logger.Error("This isn't a valid type");
            throw new Exception("This isn't a valid type");
        }
            
        if (!Enum.IsDefined(typeof(WordLevelEnum), dtoUpd.Level))
        {
            _logger.Error("This isn't a valid level");
            throw new Exception("This isn't a valid level");
        }
    }

}