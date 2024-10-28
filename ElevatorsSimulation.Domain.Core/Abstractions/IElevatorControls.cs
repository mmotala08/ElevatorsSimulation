using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Abstractions
{
    public interface IElevatorControls
    {
        void AddElevatorFloorRequest(ElevatorFloorRequest elevatorFloorRequest, int priority);
        Task DispatchAsync(CancellationToken cancellationToken);
    }
}
