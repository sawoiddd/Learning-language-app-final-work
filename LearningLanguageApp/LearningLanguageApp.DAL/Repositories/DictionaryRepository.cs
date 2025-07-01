using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        _logger.Information("Adding dictionary for UserId {UserId}", dictionary.UserId);
        await _context.Dictionaries.AddAsync(dictionary, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.Information("Added dictionary with Id {DictionaryId}", dictionary.Id);
        return dictionary;
    }

    public async Task<Dictionary> DeleteDictionaryAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var entity = await _context.Dictionaries.FindAsync(new object[] { dictionaryId }, cancellationToken);
        if (entity == null)
        {
            _logger.Warning("Dictionary with Id {DictionaryId} not found for deletion", dictionaryId);
            return null;
        }

        _context.Dictionaries.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.Information("Deleted dictionary with Id {DictionaryId}", dictionaryId);
        return entity;
    }

    public async Task<IEnumerable<Dictionary>> GetByUserDictionaryIdAsync(int userId, CancellationToken cancellationToken)
    {
        _logger.Information("Retrieving dictionaries for UserId {UserId}", userId);
        var list = await _context.Dictionaries
           .Where(d => d.UserId == userId)
           .ToListAsync(cancellationToken);
        _logger.Information("Found {Count} dictionaries for UserId {UserId}", list.Count, userId);
        return list;        
    }

    public async Task<Dictionary> GetDictionaryByIdAsync(int dictionaryId, CancellationToken cancellationToken)
    {
        var dictionary = await _context.Dictionaries
            .Include(d => d.Words)
            .FirstOrDefaultAsync(d => d.Id == dictionaryId, cancellationToken);
        if (dictionary == null)
        {
            _logger.Warning("Dictionary with Id {DictionaryId} not found", dictionaryId);
            return null;
        }
        _logger.Information("Retrieving dictionary with Id {DictionaryId}", dictionaryId);
        return dictionary;
    }

    public async Task<Dictionary> UpdateDictionaryAsync(Dictionary dictionary, CancellationToken cancellationToken)
    {
        _logger.Information("Updating dictionary with Id {DictionaryId}", dictionary.Id);
        var existing = await _context.Dictionaries.FindAsync(new object[] { dictionary.Id }, cancellationToken);
        if (existing == null)
        {
            _logger.Warning("Dictionary with Id {DictionaryId} not found for update", dictionary.Id);
            throw new KeyNotFoundException($"Dictionary with ID {dictionary.Id} not found");
        }
        existing.SourceLanguage = dictionary.SourceLanguage;
        existing.TargetLanguage = dictionary.TargetLanguage;

        // Word update logic(TEST)

        _context.Dictionaries.Update(existing);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.Information("Updated dictionary with Id {DictionaryId}", dictionary.Id);
        return existing;
    }
}
