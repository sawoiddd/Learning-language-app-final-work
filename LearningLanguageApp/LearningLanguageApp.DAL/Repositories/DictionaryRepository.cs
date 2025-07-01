using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearningLanguageApp.DAL.Repositories;

public class DictionaryRepository : IDictionaryRepository
{
    private readonly LearningLanguageAppDataContext _context;
    private readonly ILogger _logger;

    public DictionaryRepository(LearningLanguageAppDataContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Dictionary> AddDictionaryAsync(Dictionary dictionary, CancellationToken cancellationToken)
    {
        _context.Dictionaries.Add(dictionary);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information($"Added dictionary with Id {dictionary.Id} to user by id: {dictionary.UserId}");
        return dictionary;
    }

    public async Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var entity = await _context.Dictionaries.FindAsync(dictionaryId, cancellationToken);

        if (entity == null)
        {
            _logger.Error($"Dictionary with Id {dictionaryId} not found for deletion");
            return null;
        }

        _context.Dictionaries.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Deleted dictionary with Id {DictionaryId}", dictionaryId);
        return entity;
    }

    public async Task<IEnumerable<Dictionary>> GetByUserDictionaryIdAsync(int userId, CancellationToken cancellationToken)
    {
        var result = await _context.Dictionaries.Where(d => d.UserId == userId)
           .ToListAsync(cancellationToken);

        if (result == null || !result.Any())
        {
            _logger.Error($"No dictionaries found for UserId {userId}");
            return null;
        }

        _logger.Information($"Found {result.Count} dictionaries for UserId {userId}");
        return result;
    }

    public async Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var dictionary = await _context.Dictionaries.Include(d => d.Words)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == dictionaryId, cancellationToken);

        if (dictionary == null)
        {
            _logger.Error($"Dictionary with Id {dictionaryId} not found");
            return null;
        }

        _logger.Information($"Retrieving dictionary with Id {dictionaryId}");
        return dictionary;
    }

    public async Task<Dictionary> UpdateDictionaryAsync(Dictionary dictionary, CancellationToken cancellationToken)
    {
        var existing = await _context.Dictionaries.FindAsync(dictionary.Id, cancellationToken);

        if (existing == null)
        {
            _logger.Error($"Dictionary with Id {dictionary.Id} not found for update");
            return null;
        }

        existing.SourceLanguage = dictionary.SourceLanguage;
        existing.TargetLanguage = dictionary.TargetLanguage;

        _context.Dictionaries.Update(existing);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.Information("Updated dictionary with Id {DictionaryId}", dictionary.Id);
        return existing;
    }
}
