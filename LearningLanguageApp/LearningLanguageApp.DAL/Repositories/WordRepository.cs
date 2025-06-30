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

        _logger.Information("Word '{Word}' added to dictionary {DictionaryId}", word.OriginalWord, dictionaryId);
        return word;
    }


    public async Task<Word> UpdateWordAsync(Word word, CancellationToken cancellationToken)
    {
        var existing = await _context.Words.FindAsync(new object[] { word.Id }, cancellationToken)
            ?? throw new KeyNotFoundException($"Word with ID {word.Id} not found");

        existing.OriginalWord = word.OriginalWord;
        existing.Translation = word.Translation;
        existing.Type = word.Type;
        existing.Level = word.Level;
        existing.IsLearned = word.IsLearned;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Word '{WordId}' updated", word.Id);
        return existing;
    }

    public async Task<Word> DeleteWordAsync(int wordId, CancellationToken cancellationToken)
    {
        var word = await _context.Words.FindAsync(new object[] { wordId }, cancellationToken)
            ?? throw new KeyNotFoundException($"Word with ID {wordId} not found");

        _context.Words.Remove(word);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Word '{Word}' deleted", word.OriginalWord);
        return word;
    }




    public async Task<Word> LearnWordAsync(int wordId, CancellationToken cancellationToken)
    {
        var word = await _context.Words.FindAsync(new object[] { wordId }, cancellationToken)
            ?? throw new KeyNotFoundException($"Word with ID {wordId} not found");

        word.IsLearned = true;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Word '{Word}' marked as learned", word.OriginalWord);
        return word;
    }


    public async Task<IEnumerable<Word>> GetWordsByDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var words = await _context.Words
            .Where(w => w.DictionaryID == dictionaryId)
            .ToListAsync(cancellationToken);

        _logger.Information("Retrieved {Count} words from dictionary {DictionaryId}", words.Count, dictionaryId);
        return words;
    }
}
