using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverTech.Logic.Models;
using RoverTech.Logic.Parsers;
using RoverTech.Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RoverTech.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void EnsureGivenTestCaseIsSatisified()
        {
            /* expected */
            Output expected = new Output();

            expected.Rovers.Add(new Rover(new Position(new Point(1, 3), Cardinal.North)));
            expected.Rovers.Add(new Rover(new Position(new Point(5, 1), Cardinal.East)));

            /* prepare */
            const string str = "5 5\n1 2 N\nLMLMLMLMM\n3 3 E\nMMRMMRMRRM";

            using Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            using IInputParser parser = new InputParser(GetTestLogger<LogicTests>(), stream);
            using IRoverNavigatorService service = new RoverNavigatorService(GetTestLogger<LogicTests>(), parser.Parse());

            Output output = service.Navigate();

            output.Should()
                .BeEquivalentTo(expected);
        }

        [TestMethod]
        public void EnsureRoverCannotBePlacedOutOfBounds()
        {
            /* prepare */
            const string str = "5 5\n6 2 N\nLMLMLMLMM\n3 3 E\nMMRMMRMRRM";

            using Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            using IInputParser parser = new InputParser(GetTestLogger<LogicTests>(), stream);
            using IRoverNavigatorService service = new RoverNavigatorService(GetTestLogger<LogicTests>(), parser.Parse());

            service.Invoking(y => y.Navigate())
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Cannot place rover out of bounds at (6, 2).");
        }

        [TestMethod]
        public void EnsureRoverCannotMoveOutOfBounds()
        {
            /* prepare */
            const string str = "5 5\n1 1 E\nMMMMMMM\n3 3 E\nMMRMMRMRRM";

            using Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            using IInputParser parser = new InputParser(GetTestLogger<LogicTests>(), stream);
            using IRoverNavigatorService service = new RoverNavigatorService(GetTestLogger<LogicTests>(), parser.Parse());

            service.Invoking(y => y.Navigate())
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Rover has gone out of bounds at [(6, 1), East].");
        }

        [TestMethod]
        public void EnsureRoverRotatesLeftCorrectly()
        {
            /* expected */
            List<Position> expected = new List<Position>()
            {
                new Position(new Point(), Cardinal.North),
                new Position(new Point(), Cardinal.West),
                new Position(new Point(), Cardinal.South),
                new Position(new Point(), Cardinal.East),
                new Position(new Point(), Cardinal.North),
            };

            /* prepare */
            Rover rover = new Rover();

            /* act */
            foreach (Position position in expected)
            {
                rover.Position
                    .Should()
                    .BeEquivalentTo(position);

                rover.RotateLeft();
            }
        }

        [TestMethod]
        public void EnsureRoverRotatesRightCorrectly()
        {
            /* expected */
            List<Position> expected = new List<Position>()
            {
                new Position(new Point(), Cardinal.North),
                new Position(new Point(), Cardinal.East),
                new Position(new Point(), Cardinal.South),
                new Position(new Point(), Cardinal.West),
                new Position(new Point(), Cardinal.North),
            };

            /* prepare */
            Rover rover = new Rover();

            /* act */
            foreach (Position position in expected)
            {
                rover.Position
                    .Should()
                    .BeEquivalentTo(position);

                rover.RotateRight();
            }
        }

        [TestMethod]
        public void EnsureRoverMovesForwardCorrectly()
        {
            /* expected */
            List<Position> expected = new List<Position>()
            {
                new Position(new Point(0, 0), Cardinal.North),
                new Position(new Point(0, 1), Cardinal.East),
                new Position(new Point(1, 1), Cardinal.South),
                new Position(new Point(1, 0), Cardinal.West),
                new Position(new Point(0, 0), Cardinal.North),
            };

            /* prepare */
            Rover rover = new Rover();

            /* act */
            foreach (Position position in expected)
            {
                rover.Position
                    .Should()
                    .BeEquivalentTo(position);

                rover.MoveForward();
                rover.RotateRight();
            }
        }

        [TestMethod]
        public void EnsureParserInterpretsCorrectInput()
        {
            /* expected */
            Input expected = new Input()
            {
                Bounds = new Point(10, 20),
                Sequences = new List<Sequence>()
                {
                    new Sequence()
                    {
                        Position = new Position(new Point(1, 2), Cardinal.North),
                        Operations = new List<Operation>()
                        {
                            Operation.Move,
                            Operation.Move,
                            Operation.Left,
                            Operation.Right,
                            Operation.Move,
                            Operation.Move
                        }
                    },
                    new Sequence()
                    {
                        Position = new Position(new Point(0, 5), Cardinal.East),
                        Operations = new List<Operation>()
                        {
                            Operation.Right,
                            Operation.Move,
                            Operation.Move,
                            Operation.Left,
                            Operation.Move,
                            Operation.Right
                        }
                    }
                }
            };

            /* prepare */
            const string str = "10 20\n1 2 N\nMMLRMM\n0 5 E\nRMMLMR\n";

            using Stream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            using IInputParser parser = new InputParser(GetTestLogger<LogicTests>(), stream);

            /* act */
            Input input = parser.Parse();

            /* assert */
            input.Should()
                .BeEquivalentTo(expected);
        }

        private ILogger<T> GetTestLogger<T>()
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole();

                builder
                    .SetMinimumLevel(LogLevel.Debug);
            });

            return loggerFactory.CreateLogger<T>();
        }
    }
}
