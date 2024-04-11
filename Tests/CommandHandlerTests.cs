using CommandLine;
using IPLogger;
using IPLogger.Entity;
using Moq;

namespace Tests;

public class CommandHandlerTests
{
    private Mock<ILogFileReader> _logFileReaderMock;
    private Mock<ILogEntriesFilter> _logEntriesFilterMock;
    private Mock<IOutputFileWriter> _outputFileWriterMock;
    private ICommandLineParser _commandLineParser;
    private CommandHandler _commandHandler;

    [SetUp]
    public void SetUp()
    {
        _logFileReaderMock = new Mock<ILogFileReader>();
        _logEntriesFilterMock = new Mock<ILogEntriesFilter>();
        _outputFileWriterMock = new Mock<IOutputFileWriter>();
        _commandLineParser = new CommandLineParser(new Parser());

        _commandHandler = new CommandHandler(
            _logFileReaderMock.Object,
            _logEntriesFilterMock.Object,
            _outputFileWriterMock.Object,
            _commandLineParser
        );
    }

    [Test]
    public void HandleCommand_CorrectArgs_CallsAllDependencies()
    {
        var args = "--file-log test.log  --file-output test-output.log --time-start 10.04.2023 --time-end 15.04.2023";
        var logEntries = new List<LogEntry>(){new LogEntry(ipAddress:"192.168.1.2", accessTime:DateTime.Now)};
        var filteredEntries = new List<LogEntry>() { new LogEntry(ipAddress: "192.168.1.2", accessTime: DateTime.Now) };

        _logFileReaderMock.Setup(reader => reader.Read("test.log")).Returns(logEntries);
        _logEntriesFilterMock.Setup(filter => filter.Filter(logEntries, It.IsAny<Options>())).Returns(filteredEntries);

        _commandHandler.HandleCommand(args);

        _logFileReaderMock.Verify(reader => reader.Read("test.log"), Times.Once);
        _logEntriesFilterMock.Verify(filter => filter.Filter(logEntries, It.IsAny<Options>()), Times.Once);
        _outputFileWriterMock.Verify(writer => writer.Write(filteredEntries, "test-output.log"), Times.Once);
    }
}