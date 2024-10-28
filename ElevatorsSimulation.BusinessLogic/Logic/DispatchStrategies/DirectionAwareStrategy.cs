using ElevatorsSimulation.BusinessLogic.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;

namespace ElevatorsSimulation.BusinessLogic.Logic.DispatchStrategies
{
    public class DirectionAwareStrategy : IElevatorDispatchStrategy
    {
        public Elevator? AllocateElevator(List<Elevator> availableElevators, ElevatorFloorRequest request)
        {
            // Get all elevators moving in the same direction or that are idle
            var allocatedElevator = availableElevators.Where(elevator => !elevator.IsBusy)
                .Where(e => e.Direction == request.Direction || e.Direction == ElevatorDirection.None)
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor)).FirstOrDefault();

            return allocatedElevator;
        }
    }
}
