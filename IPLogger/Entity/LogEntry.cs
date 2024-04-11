namespace IPLogger.Entity;

public class LogEntry
{
    public string IPAddress { get; set; }
    public DateTime AccessTime { get; set; }

    public LogEntry()
    {
    }

    public LogEntry(string ipAddress, DateTime accessTime)
    {
        IPAddress = ipAddress;
        AccessTime = accessTime;
    }
}