using Serilog;

namespace LearningLanguageApp.Services;

public class LoggerService
{
    public static ILogger GetLogger()
    {
        var logPath = "Logs/Logs.txt";

        var logDirectory = Path.GetDirectoryName(logPath);
        Directory.CreateDirectory(logDirectory);

        return new LoggerConfiguration()
               .WriteTo.File(logPath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}{Exception}")
               .MinimumLevel.Debug()
               .CreateLogger();
    }
}
