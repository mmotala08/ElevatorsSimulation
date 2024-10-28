using ElevatorsSimulation.Domain.Core.Models.Elevators;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.BusinessLogic.Abstractions
{
    public interface IElevatorDispatchStrategy
    {
        Elevator? AllocateElevator(List<Elevator> elevators, ElevatorFloorRequest elevatorFloorRequest);
    }
}
