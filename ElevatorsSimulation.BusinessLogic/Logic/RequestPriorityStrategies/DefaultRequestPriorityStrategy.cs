using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorsSimulation.BusinessLogic.Abstractions;

namespace ElevatorsSimulation.BusinessLogic.Logic.RequestPriorityStrategies
{
    public class DefaultRequestPriorityStrategy : IElevatorRequestPriorityStrategy
    {
        public int GetElevatorRequestPriority(Elevator elevator, ElevatorFloorRequest elevatorFloorRequest)
        {
            if (elevator.IsDispatched && elevator.IsBusy && elevator.CurrentFloor != elevatorFloorRequest.Floor)
            {
                var elevatorCurrentRequest = elevator.CurrentRequest;
                if (elevatorCurrentRequest == null)
                    return 1;

                bool isMovingUp = elevatorCurrentRequest.Direction == Domain.Core.Enums.ElevatorDirection.Up;
                int currentDestination = elevatorCurrentRequest.Floor;
                int newRequestFloor = elevatorFloorRequest.Floor;

                // Calculate the distances
                int distanceToCurrentDestination = Math.Abs(currentDestination - elevator.CurrentFloor);
                int distanceToNewRequest = Math.Abs(newRequestFloor - elevator.CurrentFloor);

                // Check if the new request is on the way and if it has a higher priority
                bool isOnTheWay = (isMovingUp && newRequestFloor > elevator.CurrentFloor && newRequestFloor < currentDestination) ||
                                  (!isMovingUp && newRequestFloor < elevator.CurrentFloor && newRequestFloor > currentDestination);

                if (isOnTheWay)
                {
                    return distanceToNewRequest < distanceToCurrentDestination ? 0 : 1; // 0 = high priority, 1 = lower priority
                }
            }
            return 1;
        }
    }
}
