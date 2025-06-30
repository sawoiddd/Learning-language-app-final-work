using System.Text.Json;
using LearningLanguageApp.Services;
using Microsoft.Extensions.Configuration;

namespace LearningLanguageApp;

using Discord;
using Discord.WebSocket;


class Program
{
    static async Task Main(string[] args)
    {
        
        

        var configDiscord = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.Guilds |
                             GatewayIntents.GuildMessages |
                             GatewayIntents.MessageContent
        };

        
        
        var discordClient = new DiscordSocketClient(configDiscord);
        
        var botBaseServices = new DiscordBotBaseServices(discordClient, guildId);

        await discordClient.LoginAsync(TokenType.Bot, token);
        await discordClient.StartAsync();
        
        discordClient.Log += botBaseServices.DiscordClient_Log;
        discordClient.MessageReceived += botBaseServices.DiscordClient_MessageReceived;
        discordClient.Ready += botBaseServices.DiscordClient_Ready;
        discordClient.SlashCommandExecuted += botBaseServices.DiscordClient_SlashCommandExecuted;
        
        Console.WriteLine("Hello Discord Bot");

        Console.ReadKey();

    }
}
