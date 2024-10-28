using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Services.Services.Abstractions;
using Microsoft.Extensions.Hosting;

namespace ElevatorsSimulation
{
    public class ElevatorDispatcherHostedService : IHostedService, IDisposable
    {
        private readonly IElevatorDispatcherService _elevatorDispatcherService;
        private Task _backgroundTask;
        private CancellationTokenSource _cancellationTokenSource;
        private IBuilding _building;

        public ElevatorDispatcherHostedService(IElevatorDispatcherService elevatorDispatcherService, IBuilding building)
        {
            _elevatorDispatcherService = elevatorDispatcherService;
            _building = building;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // Start the dispatcher in the background
            _backgroundTask = Task.Run(() => ProcessQueueInBackground(_cancellationTokenSource.Token), cancellationToken);

            return Task.CompletedTask;
        }

        private async Task ProcessQueueInBackground(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Process requests while the dispatcher is running
                await _elevatorDispatcherService.HandleElevatorRequests();

            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop the background task
            _cancellationTokenSource.Cancel();

            return Task.WhenAny(_backgroundTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
