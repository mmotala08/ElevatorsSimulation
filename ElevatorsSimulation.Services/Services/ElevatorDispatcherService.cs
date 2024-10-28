using ElevatorsSimulation.BusinessLogic.Abstractions;
using ElevatorsSimulation.BusinessLogic.Logic;
using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Services.Services.Abstractions;

namespace ElevatorsSimulation.Services.Services
{
    public class ElevatorDispatcherService : IElevatorDispatcherService
    {
        private readonly object _queueLock = new object();
        private readonly IBuilding _building;
        private readonly IElevatorQueueService _elevatorQueueService;
        private readonly IElevatorDispatchStrategy _elevatorDispatchStrategy;
        private readonly IElevatorRequestPriorityStrategy _elevatorRequestPriorityStrategy;
        private readonly Dictionary<Guid, ActiveElevatorRequest> _activeRequests = new Dictionary<Guid, ActiveElevatorRequest>();

        public ElevatorDispatcherService(IBuilding building, IElevatorQueueService elevatorQueueService, IElevatorDispatchStrategy elevatorDispatchStrategy, IElevatorRequestPriorityStrategy elevatorRequestPriorityStrategy)
        {
            _building = building ?? throw new ArgumentNullException(nameof(building));
            _elevatorQueueService = elevatorQueueService ?? throw new ArgumentNullException(nameof(elevatorQueueService));
            _elevatorDispatchStrategy = elevatorDispatchStrategy ?? throw new ArgumentNullException(nameof(elevatorDispatchStrategy));
            _elevatorRequestPriorityStrategy = elevatorRequestPriorityStrategy ?? throw new ArgumentNullException(nameof(elevatorRequestPriorityStrategy));

            foreach (var elevator in _building.Elevators)
            {
                elevator.FloorReached += OnElevatorFloorReached;
            }
        }

        public async Task HandleElevatorRequests()
        {
            while (_elevatorQueueService.HasRequests())
            {
                lock (_queueLock)
                {
                    if (!_elevatorQueueService.HasRequests())
                        break;

                    var allocatedElevator = TryAllocateElevator(_elevatorQueueService.Peek());
                    if (allocatedElevator != null)
                    {
                        var request = _elevatorQueueService.Dequeue();
                        try
                        {
                            ProcessRequest(allocatedElevator, request);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to process elevator request: {ex.Message}");
                        }// Remove it from the queue if successfully allocated
                    }

                }
            }
        }

        private void ProcessRequest(Elevator allocatedElevator, ElevatorFloorRequest request)
        {

            var cancellationTokenSource = new CancellationTokenSource();
            var activeElevatorRequest = DispatchElevator(request, allocatedElevator, cancellationTokenSource);

            _activeRequests[activeElevatorRequest.Id] = activeElevatorRequest;
        }

        private Elevator? AllocateElevator(ElevatorFloorRequest elevatorFloorRequest)
        {
            // Allocate Elevator
            var allocatedElevator = _elevatorDispatchStrategy.AllocateElevator(_building.Elevators.ToList(), elevatorFloorRequest);
            if (allocatedElevator == null)
                return null;

            return _building.Elevators[allocatedElevator.ElevatorNumber - 1];
        }
        private ActiveElevatorRequest DispatchElevator(ElevatorFloorRequest elevatorFloorRequest, Elevator allocatedElevator, CancellationTokenSource cancellationTokenSource)
        {
            var activeElevatorRequest = new ActiveElevatorRequest(elevatorFloorRequest, allocatedElevator);
            allocatedElevator.AddElevatorFloorRequest(activeElevatorRequest, GetRequestPriority(allocatedElevator, activeElevatorRequest));
            if (!allocatedElevator.IsDispatched)
            {
                Task.Run(() => allocatedElevator.DispatchAsync(cancellationTokenSource.Token));
            }

            allocatedElevator.IsDispatched = true;
            return activeElevatorRequest;
        }

        private int GetRequestPriority(Elevator elevator, ActiveElevatorRequest activeElevatorRequest)
        {
            return _elevatorRequestPriorityStrategy.GetElevatorRequestPriority(elevator, activeElevatorRequest);
        }
        private void OnElevatorFloorReached(object? sender, Guid Id)
        {
            if (sender is not Elevator elevator)
                return;

            lock (_queueLock)
            {
                if (_activeRequests.TryGetValue(Id, out var completedRequest))
                {
                    Console.WriteLine($"Dispatcher: Elevator {elevator.ElevatorNumber} reached floor {completedRequest.Floor}");
                    _activeRequests.Remove(Id);

                    if (!elevator.IsBusy)
                    {
                        elevator.IsDispatched = false;
                    }
                }
                else
                {
                    Console.WriteLine($"Request with ID {Id} not found.");
                }
            }
        }

        private Elevator? TryAllocateElevator(ElevatorFloorRequest request)
        {
            return _elevatorDispatchStrategy.AllocateElevator(_building.Elevators.ToList(), request);
        }
    }
}
