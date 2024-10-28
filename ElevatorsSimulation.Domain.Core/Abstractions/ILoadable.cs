using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Abstractions
{
    public interface ILoadable
    {
        double CurrentLoad { get; }
        bool CapacityReached(double requestedCapacity) => false;
        bool Load(double loadAmount);
        bool Unload(double unloadAmount);
    }
}
