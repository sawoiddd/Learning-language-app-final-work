using Microsoft.Extensions.Configuration;
using Serilog;

namespace LearningLanguageApp.Services;

public class LoggerService
{
    public static ILogger GetLogger()
    {
        var config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false)
             .Build();

        var logPath = config["Logging:LogPath"];

        var logDirectory = Path.GetDirectoryName(logPath);
        Directory.CreateDirectory(logDirectory);

        return new LoggerConfiguration()
               .WriteTo.File(logPath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}{Exception}")
               .MinimumLevel.Debug()
               .CreateLogger();
    }
}
