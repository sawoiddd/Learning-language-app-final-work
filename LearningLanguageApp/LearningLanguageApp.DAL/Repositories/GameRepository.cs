using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace LearningLanguageApp.DAL.Repositories;

public class GameRepository : IGameRepository
{
    private readonly LearningLanguageAppDataContext _context;
    private readonly ILogger _logger;
    private readonly Random _random = new Random();

    public GameRepository(LearningLanguageAppDataContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IList<Word>> GetRandomWordsByDictionaryAsync(int dictionaryId, int count, CancellationToken cancellationToken)
    {
        var query = _context.Words.Where(w => w.DictionaryID == dictionaryId)
            .Select(w => w.Id);

        var allIds = await query.ToListAsync(cancellationToken);

        if (allIds.Count == 0)
        {
            _logger.Error($"No words found in dictionary {dictionaryId}");
            throw new KeyNotFoundException($"No words found in dictionary with ID {dictionaryId}");
        }

        if (count > allIds.Count)
        {
            count = allIds.Count;
        }
            
        var randomIds = new HashSet<int>();

        while (randomIds.Count < count)
        {
            var idx = _random.Next(0, allIds.Count);
            randomIds.Add(allIds[idx]);
        }

        var words = await _context.Words
            .Where(w => randomIds.Contains(w.Id))
            .ToListAsync(cancellationToken);

        _logger.Information($"Retrieved {words.Count} random words from dictionary {dictionaryId}");
        return words;
    }
}
