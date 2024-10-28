using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Enums.Extensions;
using ElevatorsSimulation.Domain.Core.Models.Elevators;

namespace ElevatorsSimulation.Domain.Core.Factories
{
    public class ElevatorFactory : IElevatorFactory
    {
        private readonly Dictionary<ElevatorType, Func<int, double, Elevator>> _elevatorConstructors;

        public ElevatorFactory()
        {
            _elevatorConstructors = new Dictionary<ElevatorType, Func<int, double, Elevator>>
        {
            { ElevatorType.Passenger, (elevatorNumber, capacity) => CreatePassengerElevator(elevatorNumber, capacity) },
        };
        }

        public Elevator CreateElevator(int elevatorNumber, double elevatorCapacity, ElevatorType elevatorType)
        {
            if (!_elevatorConstructors.TryGetValue(elevatorType, out var constructor))
            {
                throw new ArgumentException($"{elevatorType} is not a valid elevator type.", nameof(elevatorType));
            }

            return constructor(elevatorNumber, elevatorCapacity);
        }

        private PassengerElevator CreatePassengerElevator(int elevatorNumber, double elevatorCapacity)
        {
            if (elevatorCapacity % 1 != 0)
            {
                throw new ArgumentException("Passenger elevator capacity must be a whole number.", nameof(elevatorCapacity));
            }

            return new PassengerElevator(elevatorNumber, (int)elevatorCapacity);
        }
    }
}
