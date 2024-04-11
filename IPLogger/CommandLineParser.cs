using CommandLine;
using IPLogger.Entity;

namespace IPLogger;

public interface ICommandLineParser
{
    ParserResult<Options> ParseArguments(string[] args);
}

public class CommandLineParser : ICommandLineParser
{
    private readonly Parser _parser;

    public CommandLineParser(Parser parser)
    {
        _parser = parser;
    }

    public ParserResult<Options> ParseArguments(string[] args)
    {
        var parserResult = _parser.ParseArguments<Options>(args);
        return parserResult;
    }
}