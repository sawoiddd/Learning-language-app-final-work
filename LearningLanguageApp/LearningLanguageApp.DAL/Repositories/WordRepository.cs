using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLanguageApp.DAL.Repositories;

public class WordRepository : IWordRepository
{
    private readonly LearningLanguageAppDataContext _context;
    public WordRepository(LearningLanguageAppDataContext context)
    {
        _context = context;
    }



}
