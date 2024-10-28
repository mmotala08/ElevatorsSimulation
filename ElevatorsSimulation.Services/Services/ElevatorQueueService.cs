using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Services.Services.Abstractions;

namespace ElevatorsSimulation.Services.Services
{
    public class ElevatorQueueService : IElevatorQueueService
    {
        public bool HasRequests() => _elevatorRequestQueue.Count > 0;
        private readonly Queue<ElevatorFloorRequest> _elevatorRequestQueue = new Queue<ElevatorFloorRequest>();

        public ElevatorFloorRequest Dequeue()
        {
            return _elevatorRequestQueue.Dequeue();
        }

        public async Task Enqueue(ElevatorFloorRequest elevatorFloorRequest)
        {
            _elevatorRequestQueue.Enqueue(elevatorFloorRequest);
        }

        public ElevatorFloorRequest Peek()
        {
            return _elevatorRequestQueue.Peek();
        }
    }
}
