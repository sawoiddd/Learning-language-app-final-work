using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using Serilog;

namespace LearningLanguageApp.Services;

public class DictionaryService : IDictionaryService
{
    private readonly IDictionaryRepository _dictionaryRepository;
    private readonly ILogger  _logger;

    public DictionaryService(IDictionaryRepository dictionaryRepository,  ILogger logger)
    {
        _dictionaryRepository = dictionaryRepository;
        _logger = logger;
    }
    
    public async Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            _logger.Error($"Invalid dictionary id: {dictionaryId}");
            throw new Exception("Invalid dictionary id");
        }
        
        var dictionary = await _dictionaryRepository.GetDictionaryByIdAsync(dictionaryId, cancellationToken);

        if (dictionary == null)
        {
            _logger.Error($"Failed to find dictionary with id {dictionaryId}");
            throw new KeyNotFoundException($"Dictionary with Id {dictionaryId} not found");
        }

        _logger.Information($"Getting dictionary with id: {dictionaryId}");
        return dictionary;
    }

    public async Task<IEnumerable<Dictionary>> GetUserDictionariesAsync(int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
        {
            _logger.Error($"Invalid user id: {userId}");
            throw new Exception("Invalid user id");
        }
        
        var result = await _dictionaryRepository.GetByUserDictionaryIdAsync(userId, cancellationToken);

        if (result == null)
        {
            _logger.Error($"Failed to find dictionaries for user with id: {userId}");
            throw new KeyNotFoundException($"No dictionaries found for UserId: {userId}");
        }

        _logger.Information($"Getting dictionaries for user with id: {userId}");
        return result;
    }

    public Task<Dictionary> CreateDictionaryAsync(AddDictionaryDto dto, int userId, CancellationToken cancellationToken)
    {
        ValidationAddDto(dto);

        if (userId <= 0)
        {
            _logger.Error($"Invalid user id: {userId}");
            throw new Exception("Invalid user id");
        }

        var dict = new Dictionary()
        {
            UserId = userId,
            SourceLanguage = dto.SourceLanguage,
            TargetLanguage = dto.TargetLanguage,
            Words = new List<Word>()
        };

        return _dictionaryRepository.AddDictionaryAsync(dict, cancellationToken);       
    }

    public async Task<Dictionary> UpdateDictionaryAsync(UpdateDictionaryDto dto, CancellationToken cancellationToken)
    {
        ValidationUpdDto(dto);

        var dict = new Dictionary()
        {
            Id = dto.Id,
            SourceLanguage = dto.SourceLanguage,
            TargetLanguage = dto.TargetLanguage,
        };

        var dictionary = await _dictionaryRepository.UpdateDictionaryAsync(dict, cancellationToken);

        if (dictionary == null)
        {
            _logger.Error($"Failed to update dictionary with id: {dto.Id}");
            throw new Exception($"Dictionary with Id {dictionary.Id} not found for update");
        }

        _logger.Information($"Updating dictionary with id: {dto.Id}, source language: {dto.SourceLanguage}, target language: {dto.TargetLanguage}");
        return dictionary;
    }

    public async Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            _logger.Error($"Invalid dictionary id");
            throw new Exception("Invalid dictionary id");
        }
        
        var dictionary = await _dictionaryRepository.DeleteDictionaryAsync(dictionaryId, cancellationToken);

        if (dictionary == null)
        {
            _logger.Error($"Failed to delete dictionary with id {dictionaryId}");
            throw new KeyNotFoundException($"Dictionary with Id {dictionaryId} not found for deletion");
        }

        _logger.Information($"Deleting dictionary with id: {dictionaryId}");
        return dictionary;
    }
    
    private void ValidationAddDto(AddDictionaryDto dtoAdd)
    {
        if (string.IsNullOrWhiteSpace(dtoAdd.SourceLanguage))
        {
            _logger.Error("Source language could not be empty");
            throw new Exception("Source language could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoAdd.TargetLanguage))
        {
            _logger.Error("Target language could not be empty");
            throw new Exception("target language could not be empty");
        }
    }

    private void ValidationUpdDto(UpdateDictionaryDto dtoUpd)
    {
        if (dtoUpd.Id <= 0)
        {
            _logger.Error($"Invalid id: {dtoUpd.Id}");
            throw new Exception("Invalid id!");
        }
        if (string.IsNullOrWhiteSpace(dtoUpd.SourceLanguage))
        {
            _logger.Error("Source language could not be empty");
            throw new Exception("Source language could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoUpd.TargetLanguage))
        {
            _logger.Error("target language could not be empty");
            throw new Exception("target language could not be empty");
        } 
    }
}