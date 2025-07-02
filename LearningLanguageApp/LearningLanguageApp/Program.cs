using LearningLanguageApp.Services;

namespace LearningLanguageApp;

class Program
{
    static async Task Main(string[] args)
    {
        var botBaseServices = new DiscordBotBaseServices(LoggerService.GetLogger(), new Dependency("appsettings.json"));

        await botBaseServices.InitializeAsync();
        botBaseServices.EventsStart();
        
        Console.WriteLine("Discord bot has started and is ready");

        Console.ReadKey();

        Console.WriteLine("Discord bot is now online!");

    }
}
