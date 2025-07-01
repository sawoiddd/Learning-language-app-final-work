using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.DAL;
using LearningLanguageApp.BLL.Models;
using Exception = System.Exception;

namespace LearningLanguageApp.Services;

public class WordService :  IWordService
{
    private readonly IWordRepository _WordRepository;
    private readonly LearningLanguageAppDataContext _context;


    public WordService(IWordRepository IWordRepository)
    {
        _WordRepository = IWordRepository;
    }
    
    public Task<Word> AddWordAsync(int dictionaryId, AddWordDto dto, CancellationToken cancellationToken)
    {
        if (!ValidationAddDto(dto))
        {
            throw new Exception("Validation didn't pass word");
        }
            var word = new Word()
            {
                OriginalWord = dto.OriginalWord,
                Translation = dto.Translation,
                Type = dto.Type,
                Level = dto.Level,
                IsLearned = false,
                DictionaryID = dictionaryId,
                
            };
        
            return _WordRepository.AddWordAsync(dictionaryId, word, cancellationToken);
    }
    

    public Task<Word> UpdateWordAsync(UpdateWordDto word, CancellationToken cancellationToken)
    {
        if (ValidationUpdDto(word))
        {
            throw new Exception("Validation didn't pass word");
        }
        
        var updatedWord = new Word
            {
                Id = word.Id,
                OriginalWord = word.OriginalWord,
                IsLearned = false,
                Type = word.Type,
                Level = word.Level,
                DictionaryID = -1 
            }; 
        return _WordRepository.UpdateWordAsync(updatedWord,  cancellationToken);
            
    }

    public Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken)
    {
            if (wordId <= 0)
            {
                throw new Exception("Word id is invalid");
            }
            
            return _WordRepository.DeleteWordAsync(wordId, cancellationToken);
    }

    public Task<Word> MarkAsLearnedAsync(int wordId, CancellationToken cancellationToken)
    {

        if (wordId <= 0)
        {
            throw new Exception("Word id is invalid");
        }
    
        return _WordRepository.LearnWordAsync(wordId, cancellationToken);
    }
    
        
    

    public Task<IEnumerable<Word>> GetWordsAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        
        if (dictionaryId <= 0)
        {
            throw new Exception("Dictionary ID is invalid");
        }
        
        return _WordRepository.GetWordsByDictionaryAsync(dictionaryId, cancellationToken);        
    }
    
    private bool ValidationAddDto(AddWordDto dtoAdd)
    {
        if (string.IsNullOrWhiteSpace(dtoAdd.OriginalWord))
        {
            throw new Exception("Word could not be empty");
        }
            
        if (string.IsNullOrWhiteSpace(dtoAdd.Translation))
        {
            throw new Exception("Translation failed");
        }
            
        if (!Enum.IsDefined(typeof(WordTypeEnum), dtoAdd.Type) )
        {
            throw new Exception("This isn't a valid type");
        }
            
        if (!Enum.IsDefined(typeof(WordLevelEnum), dtoAdd.Level))
        {
            throw new Exception("This isn't a valid level");
        }
        
        return true;

    }

    private bool ValidationUpdDto(UpdateWordDto dtoUpd)
    {
        if (string.IsNullOrWhiteSpace(dtoUpd.OriginalWord))
        {
            throw new Exception("Word could not be empty");
        }
        if (string.IsNullOrWhiteSpace(dtoUpd.Translation))
        {
            throw new Exception("Translation failed");
        }
            
        if (!Enum.IsDefined(typeof(WordTypeEnum), dtoUpd.Type) )
        {
            throw new Exception("This isn't a valid type");
        }
            
        if (!Enum.IsDefined(typeof(WordLevelEnum), dtoUpd.Level))
        {
            throw new Exception("This isn't a valid level");
        }

        return true;
    }

}