using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Enums.Extensions;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace ElevatorsSimulation.Domain.Core.Models.Elevators
{
    public abstract class Elevator : IElevatorControls, ILoadable
    {
        public event EventHandler<Guid>? FloorReached;

        public ElevatorFloorRequest? CurrentRequest { get; private set; }
        public int ElevatorNumber { get; private set; }
        public virtual int CurrentFloor { get; protected set; }
        public virtual ElevatorDirection Direction { get; protected set; }
        public virtual ElevatorStatus Status { get; protected set; }
        public virtual bool IsBusy => _elevatorFloorRequests.Count > 0 || CurrentRequest != null;
        public bool IsDispatched { get; set; }
        public double CurrentLoad { get; protected set; }

        private readonly PriorityQueue<ElevatorFloorRequest, int> _elevatorFloorRequests = new();
        private bool _isHandlingRequest;

        protected Elevator(int elevatorNumber)
        {
            ElevatorNumber = elevatorNumber;
            Status = ElevatorStatus.Stationary;
            CurrentFloor = 0;
            CurrentLoad = 0;
            Direction = ElevatorDirection.None;
        }

        public virtual bool CapacityReached(double requestedCapacity) => false;
        public abstract bool Load(double loadAmount);
        public abstract bool Unload(double unloadAmount);

        public void AddElevatorFloorRequest(ElevatorFloorRequest elevatorFloorRequest, int priority)
        {
            if (elevatorFloorRequest == null) throw new ArgumentNullException(nameof(elevatorFloorRequest));
            _elevatorFloorRequests.Enqueue(elevatorFloorRequest, priority);
        }

        public virtual async Task DispatchAsync(CancellationToken cancellationToken)
        {

            while (_elevatorFloorRequests.Count > 0 && !cancellationToken.IsCancellationRequested)
            {
                if (_isHandlingRequest) break;

                SetCurrentRequest();

                if (CurrentRequest == null) break;

                _isHandlingRequest = true; // Indicate that a request is being handled
                await ProcessCurrentRequestAsync(cancellationToken);
                _isHandlingRequest = false; // Reset when done processing
            }
        }
        public ElevatorDirection GetDirection(int destinationFloor)
        {
            return destinationFloor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
        }

        private void SetCurrentRequest()
        {
            // Dequeue the next request and set it as the current request
            _elevatorFloorRequests.TryDequeue(out var nextRequest, out _);
            CurrentRequest = nextRequest;
        }

        private async Task ProcessCurrentRequestAsync(CancellationToken cancellationToken)
        {
            if (CurrentRequest == null) return;

            Direction = GetDirection(CurrentRequest.Floor);
            Status = ElevatorStatus.InMotion;
            Console.WriteLine($"Elevator {ElevatorNumber} moving - Current Floor: {CurrentFloor} | Status: {Status.GetDescription()} | Direction: {Direction.GetDescription()}");

            // Move towards the requested floor
            while (CurrentFloor != CurrentRequest.Floor && !cancellationToken.IsCancellationRequested)
            {
                await SimulateElevatorMovementAsync(CurrentRequest.Floor);
            }

            await ProcessFloorReachedAsync(CurrentRequest);
        }

        private async Task ProcessFloorReachedAsync(ElevatorFloorRequest request)
        {
            CurrentFloor = request.Floor;
            Status = ElevatorStatus.Stationary;
            Direction = ElevatorDirection.None;
            Console.WriteLine($"Elevator {ElevatorNumber} reached floor {CurrentFloor} - Status: {Status.GetDescription()} | Direction: {Direction.GetDescription()}");

            if (_elevatorFloorRequests.Count == 0)
            {
                CurrentRequest = null; // Reset after processing
            }
            FloorReached?.Invoke(this, request.Id);
            await Task.Delay(100); // Simulate door open time, if applicable
        }

        protected async Task SimulateElevatorMovementAsync(int destinationFloor)
        {
            CurrentFloor += (Direction == ElevatorDirection.Up) ? 1 : -1;
            Console.WriteLine($"Elevator {ElevatorNumber} moving - Current Floor: {CurrentFloor} | Status: {Status.GetDescription()} | Direction: {Direction.GetDescription()}");

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }


}
