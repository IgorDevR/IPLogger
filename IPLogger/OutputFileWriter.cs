using IPLogger.Entity;

namespace IPLogger;

public interface IOutputFileWriter
{
    public void Write(List<LogEntry> filteredEntries, string filePath);
}

public class OutputFileWriter : IOutputFileWriter
{
    public void Write(List<LogEntry> filteredEntries, string filePath)
    {
        var groupedEntries = filteredEntries
            .GroupBy(e => e.IPAddress)
            .Select(g => new
            {
                IPAddress = g.Key,
                Count = g.Count(),
                MinAccessTime = g.Min(e => e.AccessTime).ToString("yyyy-MM-dd HH:mm:ss"),
                MaxAccessTime = g.Max(e => e.AccessTime).ToString("yyyy-MM-dd HH:mm:ss")
            })
            .Select(e =>
                $"{e.IPAddress}: {e.Count} обращений, временной диапазон: с {e.MinAccessTime} по {e.MaxAccessTime}")
            .ToList();

        File.WriteAllLines(filePath, groupedEntries);
    }
}