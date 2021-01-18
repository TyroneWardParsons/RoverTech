using System.Collections.Generic;

namespace RoverTech.Logic.Models
{
    public class Sequence
    {
        public Position Position { get; set; }

        public IList<Operation> Operations { get; set; }

        public override string ToString()
        {
            return $"{Position} -> " +
                $"[{string.Join(", ", Operations)}]";
        }
    }
}
