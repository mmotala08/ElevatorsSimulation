using ElevatorsSimulation.BusinessLogic.Abstractions;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.BusinessLogic.Logic.DispatchStrategies
{
    public class NearestFloorStrategy : IElevatorDispatchStrategy
    {
        public Elevator? AllocateElevator(List<Elevator> elevators, ElevatorFloorRequest elevatorFloorRequest)
        {
            var allocatedElevator = elevators.Where(elevators => !elevators.IsBusy)
            .OrderBy(elevator => Math.Abs(elevator.CurrentFloor - elevatorFloorRequest.Floor))
            .FirstOrDefault();

            return allocatedElevator;
        }

    }
}
