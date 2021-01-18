using System.Collections.Generic;

namespace RoverTech.Logic.Models
{
    public class Input
    {
        public Point Bounds { get; set; }

        public IList<Sequence> Sequences { get; set; }

        public override string ToString()
        {
            return $"\nBounds:\t{Bounds}\n" +
                $"Sequences:\n{string.Join('\n', Sequences)}";
        }
    }
}
