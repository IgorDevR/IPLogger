using CommandLine;

namespace IPLogger.Entity;

public class Options
{
    [Option("file-log", Required = true)]
    public string FileLog { get; set; }

    [Option("file-output", Required = true)]
    public string FileOutput { get; set; }

    [Option("address-start", Required = false)]
    public string AddressStart { get; set; }

    [Option("address-mask", Required = false)]
    public int? AddressMask { get; set; }

    [Option("time-start", Required = true)]
    public string TimeStart { get; set; }

    [Option("time-end", Required = true)]
    public string TimeEnd { get; set; }
}