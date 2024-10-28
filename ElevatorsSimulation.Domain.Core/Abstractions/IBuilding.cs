using ElevatorsSimulation.Domain.Core.Models;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Abstractions
{
    public interface IBuilding
    {
        IReadOnlyList<Elevator> Elevators { get; }
        IReadOnlyList<Floor> Floors { get; }
    }
}
