using CommandLine;
using CommandLine.Text;
using IPLogger.Entity;

namespace IPLogger
{
    public class CommandHandler
    {
        private readonly ILogFileReader _logFileReader;
        private readonly ILogEntriesFilter _logEntriesFilter;
        private readonly IOutputFileWriter _outputFileWriter;
        private readonly ICommandLineParser _commandLineParser;

        public CommandHandler(ILogFileReader logFileReader, ILogEntriesFilter logEntriesFilter,
            IOutputFileWriter outputFileWriter, ICommandLineParser commandLineParser)
        {
            _logFileReader = logFileReader;
            _logEntriesFilter = logEntriesFilter;
            _outputFileWriter = outputFileWriter;
            _commandLineParser = commandLineParser;
        }

        public void HandleCommand(string? input)
        {
            var args = input.Split(' ');
            var result = _commandLineParser.ParseArguments(args);

            result.WithParsed(options =>
                {
                    if (options.AddressMask.HasValue && string.IsNullOrEmpty(options.AddressStart))
                    {
                        Console.WriteLine("Ошибка: параметр --address-mask требует задания параметра --address-start.");
                        return;
                    }

                    var logEntries = _logFileReader.Read(options.FileLog);

                    if (!logEntries.Any())
                        return;

                    var filteredEntries = _logEntriesFilter.Filter(logEntries, options);
                    _outputFileWriter.Write(filteredEntries, options.FileOutput);
                    Console.WriteLine("Обработка команды завершена.");
                })
                .WithNotParsed(errors =>
                {
                    var helpText = HelpText.AutoBuild(result, h =>
                    {
                        h.AdditionalNewLineAfterOption = false;
                        return HelpText.DefaultParsingErrorsHandler(result, h);
                    }, e => e);

                    Console.Error.WriteLine(helpText);
                });
        }
    }
}