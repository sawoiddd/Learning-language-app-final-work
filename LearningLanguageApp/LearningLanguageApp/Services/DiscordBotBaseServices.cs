using Discord;
using Discord.WebSocket;
using LearningLanguageApp.BLL.Dtos;
using LearningLanguageApp.BLL.Enums;
using LearningLanguageApp.BLL.Interfaces.Services;
using LearningLanguageApp.BLL.Models;
using LearningLanguageApp.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LearningLanguageApp;

public class DiscordBotBaseServices
{
    private readonly DiscordSocketClient _discordClient;
    private readonly DiscordSocketConfig _configDiscord;
    private readonly IConfigurationRoot _configJson;
    private readonly Dependency _dependency;
    private readonly ILogger _logger; 
    private readonly CancellationTokenSource  _tokenSource; 
    private readonly Dictionary<ulong, User> _loggedInUsers; 
    
    private List<Word> _gameWords = new List<Word>();
    private GameMode _gameMode;
    private readonly ulong _guildId;
    private readonly string _tokenBot;
    
    public DiscordBotBaseServices(ILogger logger, Dependency dependency)
    {
        _logger = logger;
        _dependency = dependency;
        
        _configDiscord = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds |
                             GatewayIntents.GuildMessages |
                             GatewayIntents.MessageContent
        };
        
        _configJson = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        
        _tokenBot= _configJson["DiscordSettings:Api"];
        _guildId = ulong.Parse(_configJson["DiscordSettings:GuildId"]);
        
        _discordClient = new DiscordSocketClient(_configDiscord);       
        _tokenSource = new CancellationTokenSource();
        _loggedInUsers = new Dictionary<ulong, User>();
        
        _logger.Information("DiscordBotBaseServices initialized successfully.");
    }

    public async Task InitializeAsync()
    {
        _logger.Information("Initializing Discord bot...");

        await _discordClient.LoginAsync(TokenType.Bot, _tokenBot);
        await _discordClient.StartAsync();
            
        _logger.Information("Discord bot logged in and started.");
    }

    public void EventsStart()
    {
        _discordClient.Log += DiscordClient_Log;
        _discordClient.MessageReceived += DiscordClient_MessageReceived;
        _discordClient.Ready += DiscordClient_Ready;
        _discordClient.SlashCommandExecuted += DiscordClient_SlashCommandExecuted;
        
        _logger.Information("Discord bot event handlers registered.");
    }
    
    public async Task DiscordClient_Log(LogMessage arg)
    {
        Console.WriteLine(arg.ToString());
    }
    
    public async Task DiscordClient_MessageReceived(SocketMessage arg)
    {
        _logger.Information($"Message received from {arg.Author.Username}: {arg.Content}");
    }
    
    public async Task DiscordClient_SlashCommandExecuted(SocketSlashCommand arg)
    {
        _logger.Information($"Slash command received: {arg.CommandName}");
       
        var wordService = Dependency.GetWordService();
        var translationService = Dependency.GetTranslationService();
        var dictionaryService = Dependency.GetDictionaryService();
        var authService = Dependency.GetAuthService();
        var gameService = Dependency.GetGameSerivce();

        try
        {
            if (IsUserLoggedIn(arg))
            {
                switch (arg.CommandName)
                {
                    case "create_dict":
                        {
                            _logger.Information("Executing create_dict command...");

                            await arg.DeferAsync();

                            int userId = _loggedInUsers[arg.User.Id].Id;

                            var sourceLanguage = GettingDataFromMsg(arg, "source_language");
                            var targetLanguage = GettingDataFromMsg(arg, "target_language");

                            var dictDto = new AddDictionaryDto()
                            {
                                UserId = userId,
                                SourceLanguage = sourceLanguage,
                                TargetLanguage = targetLanguage
                            };

                            var response = await dictionaryService.CreateDictionaryAsync(dictDto, userId, _tokenSource.Token);
                            var responseText = $"Dict was added:" +
                                               $"\nId: {response.Id}" +
                                               $"\nSource language: {sourceLanguage}" +
                                               $"\nTarget language: {targetLanguage}";

                            await arg.FollowupAsync(responseText);

                            _logger.Information("create_dict command executed successfully.");
                        }
                        break;

                    case "update_dict":
                        {
                            _logger.Information("Executing update_dict command...");

                            await arg.DeferAsync();

                            var sourceLanguage = GettingDataFromMsg(arg, "source_language");
                            var targetLanguage = GettingDataFromMsg(arg, "target_language");
                            int.TryParse(GettingDataFromMsg(arg, "dict_id"), out var dictId);

                            var dictDto = new UpdateDictionaryDto()
                            {
                                Id = dictId,
                                SourceLanguage = sourceLanguage,
                                TargetLanguage = targetLanguage
                            };

                            var response = await dictionaryService.UpdateDictionaryAsync(dictDto, _tokenSource.Token);
                            var responseText = $"Dict was updated:" +
                                               $"\nId: {response.Id}" +
                                               $"\nSource language: {sourceLanguage}" +
                                               $"\nTarget language: {targetLanguage}";

                            await arg.FollowupAsync(responseText);

                            _logger.Information("create_dict command executed successfully.");
                        }
                        break;

                    case "delete_dict":
                        {
                            _logger.Information("Executing delete_dict command...");

                            await arg.DeferAsync();

                            int.TryParse(GettingDataFromMsg(arg, "dict_id"), out var dictId);

                            var response = await dictionaryService.DeleteDictionaryAsync(dictId, _tokenSource.Token);
                            var responseText = $"Dict with id : {response.Id} was deleted";

                            await arg.FollowupAsync(responseText);

                            _logger.Information("delete_dict command executed successfully.");
                        }
                        break;

                    case "add_word":
                        {
                            _logger.Information("Executing add_word command...");

                            await arg.DeferAsync();

                            var wordOrig = GettingDataFromMsg(arg, "word");
                            var typeWord = GettingDataFromMsg(arg, "type");
                            var lvlWord = GettingDataFromMsg(arg, "level");
                            int.TryParse(GettingDataFromMsg(arg, "dictionary_id"), out var dictionaryId);

                            var dictionary = await dictionaryService.GetDictionaryByIdAsync(dictionaryId, _tokenSource.Token);

                            var originalTranslation = dictionary.SourceLanguage;
                            var targetLanguage = dictionary.TargetLanguage;

                            var translation = await translationService.GetTranslateAsync(wordOrig,
                                originalTranslation,
                                targetLanguage, _tokenSource.Token);

                            if (!Enum.TryParse<WordTypeEnum>(typeWord, true, out var typeWordTypeEnum) || !Enum.IsDefined(typeof(WordTypeEnum), typeWordTypeEnum))
                            {
                                throw new ArgumentException($"Invalid word type: {typeWord}");
                            }

                            if (!Enum.TryParse<WordLevelEnum>(lvlWord, true, out var typeLevelEnum) || !Enum.IsDefined(typeof(WordLevelEnum), typeLevelEnum))
                            {
                                throw new ArgumentException($"Invalid word level: {lvlWord}");
                            }

                            var wordDto = new AddWordDto()
                            {
                                OriginalWord = wordOrig,
                                Translation = translation,
                                Type = typeWordTypeEnum,
                                Level = typeLevelEnum,
                            };

                            var response = await wordService.AddWordAsync(dictionaryId, wordDto, _tokenSource.Token);
                            var textResponse = "Word added!\n" + ShowWord(response);

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("add_word command executed successfully.");
                        }
                        break;

                    case "delete_word":
                        {
                            _logger.Information("Executing delete_word command...");

                            await arg.DeferAsync();

                            int.TryParse(GettingDataFromMsg(arg, "id"), out var wordId);

                            var response = await wordService.DeleteWordAsync(wordId, _tokenSource.Token);
                            var textResponse = "Word Deleted\n" + ShowWord(response);

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("delete_word command executed successfully.");
                        }
                        break;

                    case "update_word":
                        {
                            _logger.Information("Executing update_word command...");

                            await arg.DeferAsync();

                            var wordOrig = GettingDataFromMsg(arg, "word");
                            var wordTranslation = GettingDataFromMsg(arg, "translation");
                            var typeWord = GettingDataFromMsg(arg, "type");
                            var lvlWord = GettingDataFromMsg(arg, "level");
                            int.TryParse(GettingDataFromMsg(arg, "dictionary_id"), out var dictionaryId);
                            int.TryParse(GettingDataFromMsg(arg, "id"), out var id);

                            if (!Enum.TryParse<WordTypeEnum>(typeWord, true, out var typeWordTypeEnum) || !Enum.IsDefined(typeof(WordTypeEnum), typeWordTypeEnum))
                            {
                                throw new ArgumentException($"Invalid word type: {typeWord}");
                            }

                            if (!Enum.TryParse<WordLevelEnum>(lvlWord, true, out var typeLevelEnum) || !Enum.IsDefined(typeof(WordLevelEnum), typeLevelEnum))
                            {
                                throw new ArgumentException($"Invalid word level: {lvlWord}");
                            }

                            var wordDto = new UpdateWordDto()
                            {
                                Id = id,
                                OriginalWord = wordOrig,
                                Translation = wordTranslation,
                                Type = typeWordTypeEnum,
                                Level = typeLevelEnum,
                            };

                            var response = await wordService.UpdateWordAsync(wordDto, _tokenSource.Token);
                            var textResponse = "Word updated!\n" + ShowWord(response);

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("update_word command executed successfully.");
                        }
                        break;

                    case "change_word_status":
                        {
                            _logger.Information("Executing change_word_status command...");

                            await arg.DeferAsync();

                            int.TryParse(GettingDataFromMsg(arg, "id"), out var id);

                            var response = await wordService.MarkAsLearnedAsync(id, _tokenSource.Token);
                            var textResponse = $"Word {response.Translation}({response.OriginalWord}) marked as learned, congratulations!";

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("change_word_status command executed successfully.");
                        }
                        break;

                    case "show_all_words":
                        {
                            _logger.Information("Executing show_all_words command...");

                            await arg.DeferAsync();

                            int.TryParse(GettingDataFromMsg(arg, "dictionary_id"), out var dictionaryId);

                            var response = await wordService.GetWordsAsync(dictionaryId, _tokenSource.Token);
                            var textResponse = "";

                            foreach (var word in response)
                            {
                                textResponse += ShowWord(word) + "\n\n";
                            }

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("show_all_words command executed successfully.");
                        }
                        break;

                    case "start_game":
                        {
                            _logger.Information("Executing start_game command...");

                            await arg.DeferAsync();

                            int.TryParse(GettingDataFromMsg(arg, "dictionary_id"), out var dictionaryId);
                            var gameMode = GettingDataFromMsg(arg, "game_mode");

                            if (!Enum.TryParse<GameMode>(gameMode, true, out var gameModeEnum) || !Enum.IsDefined(typeof(GameMode), gameModeEnum))
                            {
                                throw new ArgumentException($"Invalid game mode: {gameMode}");
                            }
                            _gameMode = gameModeEnum;

                            _gameWords = await gameService.StartGameAsync(dictionaryId, gameModeEnum, _tokenSource.Token);
                            string textResponse = "";

                            for (int i = 0; i < _gameWords.Count; i++)
                            {
                                var wordToShow = gameModeEnum == GameMode.OriginalToTranslation ? _gameWords[i].OriginalWord : _gameWords[i].Translation;
                                textResponse += $"{i + 1}. {wordToShow}\n";
                            }
                            await arg.FollowupAsync("Here is list of word:\n" + textResponse);

                            _logger.Information("start_game command executed successfully.");
                        }
                        break;

                    case "check_words":
                        {
                            _logger.Information("Executing check_words command...");

                            await arg.DeferAsync();

                            var words = GettingDataFromMsg(arg, "list_of_words");
                            var dict = new Dictionary<int, string>();

                            foreach (var word in words.Split(' '))
                            {
                                var wordSplited = word.Split('.');

                                int.TryParse(wordSplited[0], out var key);

                                dict[key] = wordSplited[1];
                            }

                            var response = gameService.CheckAnswers(dict, _gameMode, _gameWords);
                            var responseText = $"Results: {response.CorrectAnswers}/{response.TotalWords}";

                            await arg.FollowupAsync(responseText);

                            _logger.Information("check_words command executed successfully.");
                        }
                        break;

                    default:
                        await arg.Channel.SendMessageAsync("Unknown command.");
                        break;
                }
            }
            else 
            {
                switch (arg.CommandName)
                {
                    case "login_user":
                        {
                            _logger.Information("Executing login_user command...");

                            await arg.DeferAsync();

                            var login = GettingDataFromMsg(arg, "login");
                            var password = GettingDataFromMsg(arg, "password");

                            var logDto = new LoginUserDto()
                            {
                                Login = login,
                                Password = password
                            };

                            var response = await authService.LoginAsync(logDto, _tokenSource.Token);

                            _loggedInUsers[arg.User.Id] = response;

                            _logger.Information($"User {response.FirstName} logged in with Discord ID: {arg.User.Id}");

                            var textResponse = $"Great to see you {response.FirstName}";

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("login_user command executed successfully.");

                        }
                        break;

                    case "register_user":
                        {
                            _logger.Information("Executing register_user command...");

                            await arg.DeferAsync();

                            var firstName = GettingDataFromMsg(arg, "first_name");
                            var lastName = GettingDataFromMsg(arg, "last_name");
                            var login = GettingDataFromMsg(arg, "login");
                            var password = GettingDataFromMsg(arg, "password");
                            DateTime.TryParse(GettingDataFromMsg(arg, "date_of_birth"), out var dateOfBirth);

                            var regDto = new RegisterUserDto()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Login = login,
                                Password = password,
                                BirthdayDate = dateOfBirth
                            };

                            var response = await authService.RegisterAsync(regDto, _tokenSource.Token);
                            var textResponse = $"Welcome {response.FirstName}" +
                                               $"\nNow you have to login";

                            await arg.FollowupAsync(textResponse);

                            _logger.Information("register_user command executed successfully.");
                        }
                        break;

                    default:
                        await arg.Channel.SendMessageAsync("Invalid command or you are not logged in.");
                        break;
                }
            }  
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while executing command: {arg.CommandName}");

            await arg.Channel.SendMessageAsync(ex.Message);
        } 

    }
    
    public async Task DiscordClient_Ready()
    {
        _logger.Information("Bot is ready. Registering commands...");

        var createDictCommand = new SlashCommandBuilder()
            .WithName("create_dict")
            .WithDescription("Creates a new dictionary.")
            .AddOption("source_language", ApplicationCommandOptionType.String, "enter source language (like uk, en)", isRequired: true)
            .AddOption("target_language", ApplicationCommandOptionType.String, "enter target language (like uk, en)", isRequired: true)
            .Build();

        var updateDictCommand = new SlashCommandBuilder()
            .WithName("update_dict")
            .WithDescription("Updates a dictionary.")
            .AddOption("dict_id", ApplicationCommandOptionType.String, "enter id of dict", isRequired: true)
            .AddOption("source_language", ApplicationCommandOptionType.String, "enter source language",
                isRequired: true)
            .AddOption("target_language", ApplicationCommandOptionType.String, "enter target language",
                isRequired: true)
            .Build();
        
        var deleteDictCommand = new SlashCommandBuilder()
            .WithName("delete_dict")
            .WithDescription("Deletes a dictionary.")
            .AddOption("dict_id",  ApplicationCommandOptionType.String, "enter id of dict", isRequired: true)
            .Build();

        var addWordCommand = new SlashCommandBuilder()
            .WithName("add_word")
            .WithDescription("Add word to dictionary")
            .AddOption("word", ApplicationCommandOptionType.String, "word with original language", isRequired: true)
            .AddOption("type",  ApplicationCommandOptionType.String, "type of word (noun, verb…)", isRequired: true)
            .AddOption("level", ApplicationCommandOptionType.String, "Level of word (A1, A2, B2…)", isRequired: true)
            .AddOption("dictionary_id", ApplicationCommandOptionType.String, "Id of dictionary to which word belongs", isRequired: true)
            .Build();

        var deleteWordCommand = new SlashCommandBuilder()
            .WithName("delete_word")
            .WithDescription("enter id of word you want to delete")
            .AddOption("id", ApplicationCommandOptionType.String, "id of word", isRequired: true)
            .Build();

        var updateWordCommand = new SlashCommandBuilder()
            .WithName("update_word")
            .WithDescription("update word(enter id of word you want to update and changes fields) ")
            .AddOption("id", ApplicationCommandOptionType.String, "id of word", isRequired: true)
            .AddOption("word", ApplicationCommandOptionType.String, "corrected word", isRequired: false)
            .AddOption("translation", ApplicationCommandOptionType.String, "corrected translation", isRequired: false)
            .AddOption("type", ApplicationCommandOptionType.String, "corrected type of word", isRequired: false)
            .AddOption("level", ApplicationCommandOptionType.String, "corrected level of word", isRequired: false)
            .Build(); 

        var changeWordStatusCommand = new SlashCommandBuilder()
            .WithName("change_word_status")
            .WithDescription("Mark word as learned")
            .AddOption("id", ApplicationCommandOptionType.String, "id of word", isRequired: true)
            .Build();
        
        var showAllWordsCommand = new SlashCommandBuilder()
            .WithName("show_all_words")
            .WithDescription("show all words")
            .AddOption("dictionary_id", ApplicationCommandOptionType.String, "id of dictionary", isRequired: true)
            .Build();
        
        var loginUserCommand = new SlashCommandBuilder()
            .WithName("login_user")
            .WithDescription("login user")
            .AddOption("login", ApplicationCommandOptionType.String, "enter login", isRequired: true)
            .AddOption("password", ApplicationCommandOptionType.String, "enter password", isRequired: true)
            .Build();
        
        var registerUserCommand = new SlashCommandBuilder()
            .WithName("register_user")
            .WithDescription("register user")
            .AddOption("first_name", ApplicationCommandOptionType.String, "enter firstname", isRequired: true)
            .AddOption("last_name", ApplicationCommandOptionType.String, "enter lastname", isRequired: true)
            .AddOption("login", ApplicationCommandOptionType.String, "enter login", isRequired: true)
            .AddOption("password", ApplicationCommandOptionType.String, "enter password", isRequired: true)
            .AddOption("date_of_birth", ApplicationCommandOptionType.String, "enter birth date (yyyy-mm-dd)", isRequired: true)
            .Build();
        
        var startGameCommand = new SlashCommandBuilder()
            .WithName("start_game")
            .WithDescription("game for learning words")
            .AddOption("dictionary_id", ApplicationCommandOptionType.String, "id of dictionary ", isRequired: true)
            .AddOption("game_mode", ApplicationCommandOptionType.String, "Type: \'OriginalToTranslation\' or \'TranslationToOriginal\' ", isRequired: true)
            .Build();
        
        var checkWordsCommand = new SlashCommandBuilder()
            .WithName("check_words")
            .WithDescription("check words for mistakes")
            .AddOption("list_of_words", ApplicationCommandOptionType.String, "Enter words like \'1.word 2.word 3.word...", isRequired: true)
            .Build();

        
        await _discordClient.Rest.CreateGuildCommand(loginUserCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(registerUserCommand, _guildId);
        
        await _discordClient.Rest.CreateGuildCommand(createDictCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(updateDictCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(deleteDictCommand, _guildId);
        
        await _discordClient.Rest.CreateGuildCommand(addWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(deleteWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(updateWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(changeWordStatusCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(showAllWordsCommand, _guildId);
        
        await _discordClient.Rest.CreateGuildCommand(startGameCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(checkWordsCommand, _guildId);
        
        _logger.Information("Commands registered successfully.");
    }

    private string GettingDataFromMsg(SocketSlashCommand arg, string nameOfOption)
    {
        return arg.Data.Options.FirstOrDefault(x => x.Name == nameOfOption)?.Value.ToString();
    }

    private string ShowWord(Word word)
    {
        return $"Id: {word.Id}" +
               $"\nOriginal word: {word.OriginalWord}" +
               $"\nTranslation:  {word.Translation}" +
               $"\nType of word: {word.Type.ToString()}" +
               $"\nLevel of word: {word.Level.ToString()}" +
               $"\nStatus: {(word.IsLearned ? "Learned" : "Not learned")}" +
               $"\nDictionary id: {word.DictionaryID}";                     
    }
    
    private bool IsUserLoggedIn(SocketSlashCommand arg)
    {
        return _loggedInUsers.ContainsKey(arg.User.Id);
    }
}