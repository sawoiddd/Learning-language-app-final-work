using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace LearningLanguageApp.DAL.Repositories;

public class WordRepository : IWordRepository
{
    private readonly LearningLanguageAppDataContext _context;
    private readonly ILogger _logger;

    public WordRepository(LearningLanguageAppDataContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Word> AddWordAsync(int dictionaryId, Word word, CancellationToken cancellationToken)
    {
        word.DictionaryID = dictionaryId;

        _context.Words.Add(word);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information($"Word '{word.OriginalWord}' added to dictionary {dictionaryId}");
        return word;
    }
    public async Task<Word> UpdateWordAsync(Word word, CancellationToken cancellationToken)
    {
        var existing = await _context.Words.FindAsync(word.Id, cancellationToken);

        if (existing == null)
        {
            _logger.Error($"Word with ID {word.Id} not found for update");
            return null;
        }

        existing.OriginalWord = word.OriginalWord;
        existing.Translation = word.Translation;
        existing.Type = word.Type;
        existing.Level = word.Level;
        existing.IsLearned = word.IsLearned;
        existing.DictionaryID = existing.DictionaryID;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information($"Word '{word.Id}' updated");
        return existing;
    }
    public async Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken)
    {
        var word = await _context.Words.FindAsync(wordId, cancellationToken);

        if (word == null)
        {
            _logger.Error($"Word with ID {wordId} not found for deletion");
            return null;
        }

        _context.Words.Remove(word);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information($"Word '{word.OriginalWord}' deleted");
        return word;
    }
    public async Task<Word> LearnWordAsync(int wordId, CancellationToken cancellationToken)
    {
        var word = await _context.Words.FindAsync(wordId, cancellationToken);
        if (word == null)
        {
            _logger.Error($"Word with ID {wordId} not found for learning");
            return null;
        }   

        word.IsLearned = true;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Word '{Word}' marked as learned", word.OriginalWord);
        return word;
    }
    public async Task<IEnumerable<Word>> GetWordsByDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var words = await _context.Words.Where(w => w.DictionaryID == dictionaryId)
            .ToListAsync(cancellationToken);

        if (words == null || !words.Any())
        {
            _logger.Error($"No words found in dictionary {dictionaryId}");
            return null;
        }

        _logger.Information($"Retrieved {words.Count} words from dictionary {dictionaryId}");
        return words;
    }
}
