using Microsoft.Extensions.Logging;
using RoverTech.Logic.Models;
using RoverTech.Logic.Parsers;
using RoverTech.Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RoverTech.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input rover commands and denote the end with a 'q' character.");

            ILogger logger = CreateLogger();

            try
            {
                Execute(logger);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        static void Execute(ILogger logger)
        {
            List<string> lines = new List<string>();

            while (true)
            {
                string line = Console.ReadLine();

                if (line.Trim() == "q") break;

                lines.Add(line);
            }

            using MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(string.Join("\n", lines)));
            using IInputParser parser = new InputParser(logger, stream);
            using IRoverNavigatorService service = new RoverNavigatorService(logger, parser.Parse());

            Output output = service.Navigate();

            Console.WriteLine(output.ToString());
        }

        static ILogger CreateLogger()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole();

                builder
                    .SetMinimumLevel(LogLevel.Information);
            });

            return loggerFactory.CreateLogger<Program>();
        }
    }
}
