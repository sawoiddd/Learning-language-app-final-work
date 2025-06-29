using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Interfaces.Repositories;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;

namespace LearningLanguageApp.Services
{
    internal class WordService : IWordService
    {
        private readonly IWordRepository _IWordRepository;
        public WordService(IWordRepository wordRepository)
        {
            _IWordRepository = wordRepository;
        }

        public async Task<Word> AddWordAsync(Word word)
        {
            //Validate 
            var ordDto = new AddWordDto
            {
                OriginalWord = word.OriginalWord,
                Level = word.Level
                //allParams
            };
            await _IWordRepository.AddWordAsync(ordDto);

            throw new NotImplementedException();
        }

        public Task<string> DeleteWordAsync(int wordId)
        {
            throw new NotImplementedException();
        }

        public Task<string> LearnWordAsync(int wordId)
        {
            throw new NotImplementedException();
        }

        public Task<Word> UpdateWordAsync(Word word)
        {
            throw new NotImplementedException();
        }
    }
}
