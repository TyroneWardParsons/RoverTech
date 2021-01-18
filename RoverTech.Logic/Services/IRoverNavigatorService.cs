using RoverTech.Logic.Models;
using System;

namespace RoverTech.Logic.Services
{
    public interface IRoverNavigatorService : IDisposable
    {
        Output Navigate();
    }
}
