using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Models
{
    public class Building : IBuilding
    {
        public IReadOnlyList<Elevator> Elevators => _elevators;
        public IReadOnlyList<Floor> Floors => _floors;

        private readonly IElevatorFactory _elevatorFactory;
        private readonly List<Elevator> _elevators = new List<Elevator>();
        private readonly List<Floor> _floors = new List<Floor>();

        public Building(int numberOfElevators, double elevatorCapacity, ElevatorType elevatorType, int numberOfFloors, IElevatorFactory elevatorFactory)
        {
            _elevatorFactory = elevatorFactory ?? throw new ArgumentNullException(nameof(elevatorFactory));

            ValidateParameters(numberOfElevators, elevatorCapacity, numberOfFloors);

            CreateElevators(numberOfElevators, elevatorCapacity, elevatorType);
            CreateFloors(numberOfFloors);
        }

        private void ValidateParameters(int numberOfElevators, double elevatorCapacity, int numberOfFloors)
        {
            if (numberOfElevators <= 0)
            {
                throw new ArgumentException("Number of elevators must be greater than zero.", nameof(numberOfElevators));
            }

            if (elevatorCapacity <= 0)
            {
                throw new ArgumentException("Elevator capacity must be greater than zero.", nameof(elevatorCapacity));
            }

            if (numberOfFloors <= 0)
            {
                throw new ArgumentException("Number of floors must be greater than zero.", nameof(numberOfFloors));
            }
        }

        private void CreateElevators(int numberOfElevators, double elevatorCapacity, ElevatorType elevatorType)
        {
            for (int i = 0; i < numberOfElevators; i++)
            {
                var elevator = _elevatorFactory.CreateElevator(i + 1, elevatorCapacity, elevatorType);
                _elevators.Add(elevator);
            }
        }

        private void CreateFloors(int numberOfFloors)
        {
            for (int i = 0; i <= numberOfFloors; i++)
            {
                _floors.Add(new Floor(i));
            }
        }
    }

}
