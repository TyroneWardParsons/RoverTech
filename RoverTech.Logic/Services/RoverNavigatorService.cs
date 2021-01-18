using Microsoft.Extensions.Logging;
using RoverTech.Logic.Models;
using System;

namespace RoverTech.Logic.Services
{
    public class RoverNavigatorService : IRoverNavigatorService
    {
        private readonly ILogger logger;

        private readonly Input input;

        public RoverNavigatorService(ILogger logger, Input input)
        {
            this.logger = logger;
            this.input = input;
        }

        public Output Navigate()
        {
            Output output = new Output();

            foreach (Sequence sequence in input.Sequences)
            {
                if (OutOfBounds(sequence.Position.Point))
                {
                    throw new InvalidOperationException($"Cannot place rover out of bounds at {sequence.Position.Point}.");
                }

                Rover rover = new Rover(sequence.Position);

                foreach (Operation operation in sequence.Operations)
                {
                    switch (operation)
                    {
                        case Operation.Left:
                            rover.RotateLeft();
                            break;
                        case Operation.Right:
                            rover.RotateRight();
                            break;
                        case Operation.Move:
                            rover.MoveForward();
                            break;
                        default:
                            break;
                    }

                    if (OutOfBounds(rover.Position.Point))
                    {
                        throw new InvalidOperationException($"Rover has gone out of bounds at {rover.Position}.");
                    }
                }

                output.Rovers.Add(rover);
            }

            return output;
        }

        private bool OutOfBounds(Point point)
        {
            return point.X > input.Bounds.X || point.Y > input.Bounds.Y;
        }

        public void Dispose()
        {

        }
    }
}
