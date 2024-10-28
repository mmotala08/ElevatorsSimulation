using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Models.Elevators.Requests
{
    public class ActiveElevatorRequest : ElevatorFloorRequest
    {
        public Elevator Elevator { get; set; }

        public ActiveElevatorRequest(ElevatorFloorRequest elevatorRequest, Elevator elevator) :
            base(elevatorRequest.Floor, elevatorRequest.RequestCapacity, elevatorRequest.Direction)
        {
            Elevator = elevator;
        }
    }
}
