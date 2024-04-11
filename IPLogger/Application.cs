using IPLogger.Entity;
using Microsoft.Extensions.Configuration;

namespace IPLogger;

public class Application
{
    private readonly CommandHandler _commandHandler;
    private readonly IConfiguration _configuration;

    public Application(CommandHandler commandHandler, IConfiguration configuration)
    {
        _commandHandler = commandHandler;
        _configuration = configuration;
    }

    public void Run(string[] args)
    {
        Console.WriteLine(
            "Введите команды. Для загрузки из файла конфигурации введите 'load-config'. Для выхода введите 'exit'.");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (string.Equals(input, "load-config", StringComparison.OrdinalIgnoreCase))
            {
                Options options = _configuration.Get<Options>();

                string? configArgs =
                    $"--file-log={options.FileLog} " +
                    $"--file-output={options.FileOutput} " +
                    $"--address-start={options.AddressStart} " +
                    $"--address-mask={options.AddressMask} " +
                    $"--time-start={options.TimeStart} " +
                    $"--time-end={options.TimeEnd} ";

                _commandHandler.HandleCommand(configArgs);
            }
            else if (!string.IsNullOrWhiteSpace(input))
            {
                _commandHandler.HandleCommand(input);
            }
        }

        Console.WriteLine("Завершение работы программы.");
    }
}