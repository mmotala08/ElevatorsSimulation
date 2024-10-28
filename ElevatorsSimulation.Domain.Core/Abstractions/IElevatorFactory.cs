using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Abstractions
{
    public interface IElevatorFactory
    {
        Elevator CreateElevator(int elevatorNumber, double elevatorCapacity, ElevatorType elevatorType);
    }
}
