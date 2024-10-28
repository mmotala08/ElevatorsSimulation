// See https://aka.ms/new-console-template for more information

using ElevatorsSimulation;
using ElevatorsSimulation.BusinessLogic.Abstractions;
using ElevatorsSimulation.BusinessLogic.Logic.DispatchStrategies;
using ElevatorsSimulation.BusinessLogic.Logic.RequestPriorityStrategies;
using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Factories;
using ElevatorsSimulation.Domain.Core.Models;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Services.Services;
using ElevatorsSimulation.Services.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


Console.WriteLine("Elevator Simulation App");
int numberOfElevators = 0;
int numberOfFloors = 0;
int elevatorCapacity = 0;
do
{
    Console.WriteLine("Enter the number of elevators in the building.");
    if (!int.TryParse(Console.ReadLine(), out numberOfElevators) || numberOfElevators <= 0)
    {
        Console.WriteLine("Invalid input. Please enter a valid positive integer");
    }
}
while (numberOfElevators <= 0);

do
{
    Console.WriteLine("Enter the number of floors in the building excluding the ground floor (Ground Floor is 0.)");
    if (!int.TryParse(Console.ReadLine(), out numberOfFloors) || numberOfFloors <= 0)
    {
        Console.WriteLine("Invalid input. Please enter a valid positive integer");
    }
}

while (numberOfFloors <= 0);

do
{
    Console.WriteLine("Enter the capacity of the elevators.");
    if (!int.TryParse(Console.ReadLine(), out elevatorCapacity) || elevatorCapacity <= 0)
    {
        Console.WriteLine("Invalid input. Please enter a valid positive integer");
    }
}

while (elevatorCapacity <= 0);

var builder = Host.CreateDefaultBuilder(args);


builder.ConfigureServices((context, services) =>
{
    services.AddTransient<IElevatorRequestPriorityStrategy, DefaultRequestPriorityStrategy>();
    services.AddTransient<IElevatorDispatchStrategy, NearestFloorStrategy>();
    services.AddSingleton<IElevatorQueueService, ElevatorQueueService>();
    services.AddTransient<IElevatorDispatcherService, ElevatorDispatcherService>();
    services.AddTransient<IElevatorFactory, ElevatorFactory>();
    services.AddSingleton<IBuilding>(provider =>
    {
        var elevatorFactory = provider.GetRequiredService<IElevatorFactory>();
        return new Building(numberOfElevators, elevatorCapacity, ElevatorType.Passenger, numberOfFloors, elevatorFactory);
    });
    services.AddHostedService<ElevatorDispatcherHostedService>();
});

var host = builder.Build();

_ = Task.Run(() => RunElevatorRequestsAsync(host));

await host.RunAsync();


async Task RunElevatorRequestsAsync(IHost host)
{
    //var elevatorQueueService = host.Services.GetRequiredService<IElevatorQueueService>();
    //await elevatorQueueService.Enqueue(new ElevatorsSimulation.Domain.Core.Models.Elevators.Requests.ElevatorFloorRequest(5, 2, (ElevatorDirection.Up)));
    //await Task.Delay(TimeSpan.FromSeconds(2));
    //await elevatorQueueService.Enqueue(new ElevatorsSimulation.Domain.Core.Models.Elevators.Requests.ElevatorFloorRequest(5, 2, (ElevatorDirection.Up)));
    //await Task.Delay(TimeSpan.FromSeconds(2));
    //await elevatorQueueService.Enqueue(new ElevatorsSimulation.Domain.Core.Models.Elevators.Requests.ElevatorFloorRequest(6, 2, (ElevatorDirection.Up)));
    //await Task.Delay(TimeSpan.FromSeconds(2));
    //await elevatorQueueService.Enqueue(new ElevatorsSimulation.Domain.Core.Models.Elevators.Requests.ElevatorFloorRequest(17, 2, (ElevatorDirection.Up)));
    //await Task.Delay(TimeSpan.FromSeconds(2));
    //await elevatorQueueService.Enqueue(new ElevatorsSimulation.Domain.Core.Models.Elevators.Requests.ElevatorFloorRequest(1, 2, (ElevatorDirection.Down)));

    var elevatorQueueService = host.Services.GetRequiredService<IElevatorQueueService>();
    int option;

    do
    {
        Console.WriteLine("Select an option:\n 1. Request an elevator\n 2. Exit");

        if (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2))
        {
            Console.WriteLine("Invalid input. Please enter 1 or 2.");
            continue;
        }

        if (option == 1)
        {
            Console.WriteLine($"Enter a floor number between [0] and [{numberOfFloors}]:");
            if (!int.TryParse(Console.ReadLine(), out int floorNumber) || floorNumber < 0 || floorNumber > numberOfFloors)
            {
                Console.WriteLine($"Invalid floor number.Please enter a floor number between [0] and [{numberOfFloors}]:\"");
                continue;
            }

            Console.WriteLine("Would you like to move:\n 1. Up\n 2. Down");
            if (!int.TryParse(Console.ReadLine(), out int directionInput) || (directionInput != 1 && directionInput != 2))
            {
                Console.WriteLine("Invalid direction. Please enter 1 or 2.");
                continue;
            }
            var direction = (ElevatorDirection)directionInput;

            Console.WriteLine("Enter the number of people waiting:");
            if (!int.TryParse(Console.ReadLine(), out int numberOfPassengers) || numberOfPassengers < 1)
            {
                Console.WriteLine("Invalid number of passengers. Please enter a positive integer.");
                continue;
            }

            var elevatorRequest = new ElevatorFloorRequest(floorNumber, numberOfPassengers, direction);
            await elevatorQueueService.Enqueue(elevatorRequest);
            Console.WriteLine("Elevator requested successfully.");
        }

    } while (option != 2);

    Console.WriteLine("Exiting application.");

}