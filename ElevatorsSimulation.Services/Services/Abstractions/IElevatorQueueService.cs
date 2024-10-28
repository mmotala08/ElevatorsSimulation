using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Services.Services.Abstractions
{
    public interface IElevatorQueueService
    {
        Task Enqueue(ElevatorFloorRequest elevatorRequest);

        ElevatorFloorRequest Dequeue();
        ElevatorFloorRequest Peek();

        bool HasRequests();
    }
}
