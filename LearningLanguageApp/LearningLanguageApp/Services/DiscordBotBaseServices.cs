using Discord;
using Discord.WebSocket;
using LearningLanguageApp.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace LearningLanguageApp;

public class DiscordBotBaseServices
{
    private readonly DiscordSocketClient _discordClient;
    private  readonly DiscordSocketConfig _configDiscord;
    private readonly IConfigurationRoot _configJson;
    private readonly ILogger _logger;
    private readonly ulong _guildId;
    private readonly string _tokenBot;
    
    
    public DiscordBotBaseServices(ILogger logger)
    {
        _logger = logger;
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
    }

    public async Task InitializeAsync()
    {
        await _discordClient.LoginAsync(TokenType.Bot, _tokenBot);
        await _discordClient.StartAsync();
    }

    public void EventsStart()
    {
        _discordClient.Log += DiscordClient_Log;
        _discordClient.MessageReceived += DiscordClient_MessageReceived;
        _discordClient.Ready += DiscordClient_Ready;
        _discordClient.SlashCommandExecuted += DiscordClient_SlashCommandExecuted;
    }
    
    public async Task DiscordClient_Log(LogMessage arg)
    {
        Console.WriteLine(arg.ToString());
    }
    
    public async Task DiscordClient_MessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot || string.IsNullOrEmpty(arg.Content)) return;
        await arg.Channel.SendMessageAsync(arg.Content.ToString());
    }
    
    
    public async Task DiscordClient_SlashCommandExecuted(SocketSlashCommand arg)
    {
        switch (arg.CommandName)
        {
            case "add_word":
                //
                break;
            case "delete_word":
                //
                break;
            case "update_word":
                //
                break;
            case "change_word_status":
                //
                break;
            case "show_all_words":
                await arg.Channel.SendMessageAsync("test");
                //
                break;
            case "login_user":
                //
                break;
            case "register_user":
                //
                break;
            default:
                await arg.Channel.SendMessageAsync("Unknown command.");
                break;
        }
    }
    
    public async Task DiscordClient_Ready()
    {
        var addWordCommand = new SlashCommandBuilder()
            .WithName("add_word")
            .WithDescription("Add word to dictionary")
            .AddOption("word", ApplicationCommandOptionType.String, "word with original language", isRequired: true)
            .AddOption("type",  ApplicationCommandOptionType.String, "type of word (noun, verb…)", isRequired: true)
            .AddOption("level", ApplicationCommandOptionType.String, "Level of word (A1, A2, B2…)", isRequired: false)
            .Build();

        var deleteWordCommand = new SlashCommandBuilder()
            .WithName("delete_word")
            .WithDescription("enter id of word you want to delete")
            .AddOption("id", ApplicationCommandOptionType.String, "1", isRequired: true)
            .Build();

        var updateWordCommand = new SlashCommandBuilder()
            .WithName("update_word")
            .WithDescription("update word(enter id of word you want to update and changes fields) ")
            .AddOption("id", ApplicationCommandOptionType.String, "1", isRequired: true)
            .AddOption("word", ApplicationCommandOptionType.String, "corrected word", isRequired: false)
            .AddOption("translation", ApplicationCommandOptionType.String, "corrected translation", isRequired: false)
            .AddOption("type", ApplicationCommandOptionType.String, "corrected type of word", isRequired: false)
            .AddOption("level", ApplicationCommandOptionType.String, "corrected level of word", isRequired: false)
            .Build(); //id  

        var changeWordStatusCommand = new SlashCommandBuilder()
            .WithName("change_word_status")
            .WithDescription("change word status(learned/not learned)")
            .AddOption("status", ApplicationCommandOptionType.Boolean, "true or false", isRequired: true)
            .Build();
        
        var showAllWordsCommand = new SlashCommandBuilder()
            .WithName("show_all_words")
            .WithDescription("show all words")
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
            .AddOption("login", ApplicationCommandOptionType.String, "enter login", isRequired: true)
            .AddOption("password", ApplicationCommandOptionType.String, "enter password", isRequired: true)
            .Build();
        
        await _discordClient.Rest.CreateGuildCommand(loginUserCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(registerUserCommand, _guildId);
        
        await _discordClient.Rest.CreateGuildCommand(addWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(deleteWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(updateWordCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(changeWordStatusCommand, _guildId);
        await _discordClient.Rest.CreateGuildCommand(showAllWordsCommand, _guildId);
        
    }
}