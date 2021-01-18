using Microsoft.Extensions.Logging;
using RoverTech.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoverTech.Logic.Parsers
{
    public class InputParser : IInputParser
    {
        private readonly ILogger _logger;

        private readonly StreamReader _reader;

        public InputParser(ILogger logger, Stream stream)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reader = new StreamReader(stream ?? throw new ArgumentNullException(nameof(stream)));
        }

        public Input Parse()
        {
            Input input = new Input()
            {
                Bounds = ParsePlateauSize(),
                Sequences = new List<Sequence>()
            };

            do
            {
                input.Sequences.Add(ParseRoverInstructions());
            } while (!_reader.EndOfStream);

            _logger.LogDebug($"{input}");

            return input;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        private Point ParsePlateauSize()
        {
            string line = CheckAndReadLine();
            string[] values = line.Split(null);

            if (values.Length != 2)
            {
                throw new InvalidDataException("Invalid number of plateau size arguments given.");
            }

            if (int.TryParse(values[0], out int x)
                && int.TryParse(values[1], out int y))
            {
                Point point = new Point(x, y);
                return point;
            }
            else
            {
                throw new InvalidDataException("Unable to parse given plateau size arguments.");
            }
        }

        private Sequence ParseRoverInstructions()
        {
            return new Sequence()
            {
                Position = ParseRoverPosition(),
                Operations = ParseRoverCommands()
            };
        }

        private Position ParseRoverPosition()
        {
            string line = CheckAndReadLine();
            string[] values = line.Split(null);

            if (values.Length != 3)
            {
                throw new InvalidDataException("Invalid number of rover position arguments given.");
            }

            if (int.TryParse(values[0], out int x)
                && int.TryParse(values[1], out int y)
                && TryParseCardinal(values[2], out Cardinal cardinal))
            {
                Position position = new Position(new Point(x, y), cardinal);
                return position;
            }
            else
            {
                throw new InvalidDataException("Unable to parse given rover position arguments.");
            }
        }

        private IList<Operation> ParseRoverCommands()
        {
            string line = CheckAndReadLine();
            string[] values = line.Split(null);

            if (values.Length != 1)
            {
                throw new InvalidDataException("Invalid number of rover command arguments given.");
            }

            IList<Operation> operations = ParseOperations(values[0].Trim());

            return operations;
        }

        private bool TryParseCardinal(string value, out Cardinal cardinal)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                cardinal = default;
                return false;
            }

            char c = value
                .FirstOrDefault();

            cardinal = LookupCardinal(c);

            return true;
        }

        private IList<Operation> ParseOperations(string value)
        {
            List<Operation> operations = new List<Operation>();

            foreach (char c in value)
            {
                operations.Add(LookupOperation(c));
            }

            return operations;
        }

        private Cardinal LookupCardinal(char c)
        {
            return c switch
            {
                'N' => Cardinal.North,
                'E' => Cardinal.East,
                'S' => Cardinal.South,
                'W' => Cardinal.West,
                _ => throw new InvalidDataException($"Invalid cardinal character given. (N, E, S, W) supported. {c} found."),
            };
        }

        private Operation LookupOperation(char c)
        {
            return c switch
            {
                'L' => Operation.Left,
                'R' => Operation.Right,
                'M' => Operation.Move,
                _ => throw new InvalidDataException($"Invalid operation character given. (L, R, M) supported. {c} found.")
            };
        }

        private string CheckAndReadLine()
        {
            if (_reader.EndOfStream)
            {
                throw new InvalidDataException("Unexpected end of stream while parsing input.");
            }

            return _reader.ReadLine();
        }
    }
}
