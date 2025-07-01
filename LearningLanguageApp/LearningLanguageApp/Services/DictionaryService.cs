
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
    
    public Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            throw new Exception("Invalid dictionary id");
        }
        
        return _dictionaryRepository.GetDictionaryByIdAsync(dictionaryId, cancellationToken);
    }

    public Task<IEnumerable<Dictionary>> GetUserDictionariesAsync(int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
        {
            throw new Exception("Invalid user id");
        }
        
        return _dictionaryRepository.GetByUserDictionaryIdAsync(userId, cancellationToken);
    }

    public Task<Dictionary> CreateDictionaryAsync(AddDictionaryDto dto, int userId, CancellationToken cancellationToken)
    {
        ValidationAddDto(dto);

        if (userId <= 0)
        {
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

    public Task<Dictionary> UpdateDictionaryAsync(UpdateDictionaryDto dto, CancellationToken cancellationToken)
    {
        ValidationUpdDto(dto);

        var dict = new Dictionary()
        {
            Id = dto.Id,
            SourceLanguage = dto.SourceLanguage,
            TargetLanguage = dto.TargetLanguage,
            //adding words in DAL
        };
        
        return _dictionaryRepository.UpdateDictionaryAsync(dict, cancellationToken);
    }

    public Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        if (dictionaryId <= 0)
        {
            throw new Exception("Invalid dictionary id");
        }
        
        return _dictionaryRepository.DeleteDictionaryAsync(dictionaryId, cancellationToken);
    }
    
    private void ValidationAddDto(AddDictionaryDto dtoAdd)
    {
        if (string.IsNullOrWhiteSpace(dtoAdd.SourceLanguage))
        {
            throw new Exception("Source language could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoAdd.TargetLanguage))
        {
            throw new Exception("target language could not be empty");
        }
    }

    private void ValidationUpdDto(UpdateDictionaryDto dtoUpd)
    {
        if (dtoUpd.Id <= 0)
        {
            throw new Exception("Invalid id!");
        }
        if (string.IsNullOrWhiteSpace(dtoUpd.SourceLanguage))
        {
            throw new Exception("Source language could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoUpd.TargetLanguage))
        {
            throw new Exception("target language could not be empty");
        }
        
        
    }
}