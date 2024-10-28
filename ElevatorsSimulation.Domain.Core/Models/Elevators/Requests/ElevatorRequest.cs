using ElevatorsSimulation.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Models.Elevators.Requests
{
    public class ElevatorFloorRequest
    {
        public Guid Id { get; private set; }
        public int Priority { get; private set; }
        public int Floor => _floor;
        public ElevatorDirection Direction => _direction;
        public double RequestCapacity => _requestCapacity;

        private readonly int _floor;
        private readonly ElevatorDirection _direction;
        private readonly double _requestCapacity;

        public ElevatorFloorRequest(int floor, double requestCapacity, ElevatorDirection direction)
        {
            Id = Guid.NewGuid();
            _floor = floor;
            _direction = direction;
            _requestCapacity = requestCapacity;
        }

        public ElevatorFloorRequest(int floor, double requestCapacity, int priority, ElevatorDirection direction)
        {
            Id = Guid.NewGuid();
            Priority = priority;
            _floor = floor;
            _direction = direction;
            _requestCapacity = requestCapacity;
        }

    }
}
