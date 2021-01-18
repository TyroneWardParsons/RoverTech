using RoverTech.Logic.Models;
using System;

namespace RoverTech.Logic.Parsers
{
    public interface IInputParser : IDisposable
    {
        Input Parse();
    }
}
