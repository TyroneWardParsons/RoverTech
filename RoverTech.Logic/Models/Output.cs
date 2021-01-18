using System;
using System.Collections.Generic;
using System.Text;

namespace RoverTech.Logic.Models
{
    public class Output
    {
        public IList<Rover> Rovers { get; set; }

        public Output()
        {
            Rovers = new List<Rover>();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (Rover rover in Rovers)
            {
                builder.AppendLine($"{rover.Position.Point.X} {rover.Position.Point.Y} {GetDirectionChar(rover.Position.Direction)}");
            }

            return builder.ToString();
        }

        private char GetDirectionChar(Cardinal cardinal)
        {
            return cardinal switch
            {
                Cardinal.North => 'N',
                Cardinal.East => 'E',
                Cardinal.South => 'S',
                Cardinal.West => 'W',
                _ => throw new ArgumentException(nameof(cardinal)),
            };
        }
    }
}
