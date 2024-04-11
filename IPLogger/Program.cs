using CommandLine;
using IPLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddScoped<Application>()
    .AddScoped<CommandHandler>()
    .AddScoped<Parser>()
    .AddScoped<ILogFileReader, LogFileReader>()
    .AddScoped<ILogEntriesFilter, LogEntriesFilter>()
    .AddScoped<IOutputFileWriter, OutputFileWriter>()
    .AddScoped<ICommandLineParser, CommandLineParser>()
    .BuildServiceProvider();

var app = serviceProvider.GetService<Application>();
app.Run(args);