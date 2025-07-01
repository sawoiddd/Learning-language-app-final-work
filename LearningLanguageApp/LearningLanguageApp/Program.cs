using LearningLanguageApp.Services;

namespace LearningLanguageApp;

class Program
{
    static async Task Main(string[] args)
    {
        var botBaseServices = new DiscordBotBaseServices(LoggerService.GetLogger());

        await botBaseServices.InitializeAsync();
        botBaseServices.EventsStart();
        
        Console.WriteLine("Hello Discord Bot");

        Console.ReadKey();

    }
}
