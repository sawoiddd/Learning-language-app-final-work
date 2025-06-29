using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace LearningLanguageApp.DAL.Repositories;

public class WordRepository : IWordRepository
{
    private readonly LearningLanguageAppDataContext _context;
    private readonly ILogger _logger;
    public WordRepository(LearningLanguageAppDataContext context)
    {
        _context = context;
    }
    public async Task<Word> AddWordAsync(AddWordDto word) 
    {
        try
        {
            var wordNew = new Word
            {
                OriginalWord = word.OriginalWord,
                Translation = word.Translation,
                Type = word.Type,
                Level = word.Level,
                IsLearned = false
            };

            await _context.Words.AddAsync(wordNew);
            await _context.SaveChangesAsync();

            _logger.Information($"Word '{word.OriginalWord}' added successfully");
            return wordNew;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error adding word");
            throw;
        }

    }

    public async Task<Word> UpdateWordAsync(Word word)
    {
        try
        {
            var existing = await _context.Words.FindAsync(word.Id);

            if (existing == null)
            {

                _logger.Warning($"Word with ID {word.Id} not found");
                throw new Exception($"Word with ID {word.Id} not found");
            }

            existing.OriginalWord = word.OriginalWord;
            existing.Translation = word.Translation;
            existing.Type = word.Type;
            existing.Level = word.Level;
            existing.IsLearned = word.IsLearned;

            await _context.SaveChangesAsync();
            _logger.Information($"Word '{word.Id}' updated successfully");
            return existing;

        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while updating word '{word.Id}'");
            throw;

        }

    }
    public async Task<string> DeleteWordAsync(int wordId)
    {
        try
        {
            var word = await _context.Words.FindAsync(wordId);

            if (word == null)
            {
                _logger.Information($"Word with ID '{wordId}' not found for delete ");
                return $"Word with ID {wordId} not found.";
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();

            _logger.Information($"Word '{word.OriginalWord}' deleted successfully");
            return $"Word '{word.OriginalWord}' successfully deleted";

        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error deleting word '{wordId}'");
            return $"Error deleting word '{wordId}'";
        }

    }
    public async Task<string> LearnWordAsync(int wordId)
    {
        try
        {
            var word = await _context.Words.FindAsync(wordId);
            if (word == null)
            {
                _logger.Information($"Word with ID '{word}' not found for learnt ");
                return $"Word with ID {wordId} not found.";
            }
            word.IsLearned = true;
            await _context.SaveChangesAsync();

            _logger.Information($"Word '{word.OriginalWord}' learned successfully");
            return $"Word '{word.OriginalWord}' marked as learned";
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error marking word ID {wordId} as learned.");
            return $"Error occurred while updating word with ID {wordId}.";
        }

    }
    public async Task<string> GetTranslateAsync(string word) 
    {
        try
        {
            var translation = await _context.Words
                .FirstOrDefaultAsync(w => w.OriginalWord.ToLower() == word.ToLower());

            if (translation == null)
            {
                _logger.Information($"Translation for '{word}' not found for getting translation");
                return $"Translation for '{word}' not found";
            }

            _logger.Information($"Translation for word '{word}' retrieved successfully");
            return translation.Translation;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while getting translation for '{word}'");
            return $"Error while getting translation for '{word}'";
        }

    }


}
