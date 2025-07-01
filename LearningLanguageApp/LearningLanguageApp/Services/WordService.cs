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
    private readonly IWordRepository _IWordRepository;
    private readonly LearningLanguageAppDataContext _context;


    public WordService(IWordRepository IWordRepository)
    {
        _IWordRepository = IWordRepository;
    }
    
    public Task<Word> AddWordAsync(int dictionaryId, AddWordDto dto, CancellationToken cancellationToken)
    {
        if (ValidationWordDto(dto))
        {
            var word = new Word()
            {
                OriginalWord = dto.OriginalWord,
                Translation = dto.Translation,
                Type = dto.Type,
                Level = dto.Level,
                IsLearned = false,
                DictionaryID = dictionaryId,
                
            };
        
            return _IWordRepository.AddWordAsync(dictionaryId, word, cancellationToken);
        }
            
            
        throw new Exception("Validation didn't pass word");
            
        
    }
    

    public Task<Word> UpdateWordAsync(UpdateWordDto word, CancellationToken cancellationToken)
    {
        if (ValidationWordDto(word))
        {
            var updatedWord = new Word
            {
                OriginalWord = word.OriginalWord,
                Translation = word.Translation,
                Type = word.Type,
                Level = word.Level,
            }; //mb fixing later
            return _IWordRepository.UpdateWordAsync(updatedWord,  cancellationToken);
        }

        throw new Exception("Validation didn't pass word");


        
    }

    public Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken)
    {
            if (wordId <= 0)
            {
                throw new Exception("Word id is invalid");
            }
            
            return _IWordRepository.DeleteWordAsync(wordId, cancellationToken);
    }

    public Task<Word> MarkAsLearnedAsync(int wordId, CancellationToken cancellationToken)
    {

        if (wordId <= 0)
        {
            throw new Exception("Word id is invalid");
        }
    
        return _IWordRepository.LearnWordAsync(wordId, cancellationToken);
    }
    
        
    

    public Task<IEnumerable<Word>> GetWordsAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        
        if (dictionaryId <= 0)
        {
            throw new Exception("Dictionary ID is invalid");
        }
        
        return _IWordRepository.GetWordsByDictionaryAsync(dictionaryId, cancellationToken);        
    }

    private bool ValidationWordDto<T>(T dto)
    {
        
        if (dto is AddWordDto dtoAdd)
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
            
        }
        
        else if (dto is UpdateWordDto dtoUpd)
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
        }
        
        return true;
    }
}