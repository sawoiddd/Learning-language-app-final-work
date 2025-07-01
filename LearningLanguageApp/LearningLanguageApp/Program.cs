using System.Text.Json;
using LearningLanguageApp.Services;
using Microsoft.Extensions.Configuration;

namespace LearningLanguageApp;

using Discord;
using Discord.WebSocket;
using LearningLanguageApp.DAL;

class Program
{
    static async Task Main(string[] args)
    {
        var botBaseServices = new DiscordBotBaseServices();

        await botBaseServices.InitializeAsync();
        botBaseServices.EventsStart();
        
        Console.WriteLine("Hello Discord Bot");

        Console.ReadKey();

    }
}
