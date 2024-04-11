using System.Globalization;
using System.Net;
using IPLogger.Entity;

namespace IPLogger;

public interface ILogEntriesFilter
{
    public List<LogEntry> Filter(List<LogEntry> entries, Options options);
}

public class LogEntriesFilter : ILogEntriesFilter
{
    public List<LogEntry> Filter(List<LogEntry> entries, Options options)
    {
        var startTime = DateTime.ParseExact(options.TimeStart, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        var endTime = DateTime.ParseExact(options.TimeEnd, "dd.MM.yyyy", CultureInfo.InvariantCulture).AddDays(1)
            .AddTicks(-1);

        return entries
            .Where(e => e.AccessTime >= startTime && e.AccessTime <= endTime)
            .Where(e => string.IsNullOrEmpty(options.AddressStart) ||
                        IsInRange(e.IPAddress, options.AddressStart, options.AddressMask))
            .ToList();
    }

    public bool IsInRange(string ipAddress, string startAddress, int? mask)
    {
        if (mask == null)
        {
            return true;
        }

        var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
        var startIp = IPAddress.Parse(startAddress).GetAddressBytes();
        uint ipAsInt = BitConverter.ToUInt32(ip.Reverse().ToArray(), 0);
        uint startIpAsInt = BitConverter.ToUInt32(startIp.Reverse().ToArray(), 0);
        uint maskAsInt = 0xFFFFFFFF << (32 - mask.Value);

        bool isInSubnet = (ipAsInt & maskAsInt) == (startIpAsInt & maskAsInt);
        return isInSubnet;
    }
}