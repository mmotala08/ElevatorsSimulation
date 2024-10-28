using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.BusinessLogic.Abstractions
{
    public interface IElevatorRequestPriorityStrategy
    {
        public int GetElevatorRequestPriority(Elevator elevator, ElevatorFloorRequest elevatorFloorRequest);
    }
}
