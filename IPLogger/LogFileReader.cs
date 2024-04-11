using System.Globalization;
using System.Net;
using IPLogger.Entity;

namespace IPLogger;

public interface ILogFileReader
{
    public List<LogEntry> Read(string filePath);
}
public class LogFileReader : ILogFileReader
{
    public List<LogEntry> Read(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Файл по указанному пути не существует!");
            return new List<LogEntry>();
        }
           

        var logEntries = new List<LogEntry>();
        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(new[] { ':' }, 2);
            if (parts.Length == 2 && IPAddress.TryParse(parts[0], out var ipAddress))
            {
                try
                {
                    logEntries.Add(new LogEntry
                    {
                        IPAddress = parts[0],
                        AccessTime = DateTime.ParseExact(parts[1].Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    });
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"Ошибка при разборе даты в строке: {line}. Ошибка: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Невозможно разобрать строку: {line}");
            }
        }

        return logEntries;
    }
}