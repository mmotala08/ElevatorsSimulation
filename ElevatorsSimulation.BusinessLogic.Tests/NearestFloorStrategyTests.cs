using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.BusinessLogic.Tests
{
    using ElevatorsSimulation.BusinessLogic.Logic.DispatchStrategies;
    using ElevatorsSimulation.Domain.Core.Enums;
    using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
    using ElevatorsSimulation.Domain.Core.Models.Elevators;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class NearestFloorStrategyTests
    {
        private NearestFloorStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _strategy = new NearestFloorStrategy();
        }

        [Test]
        public void AllocateElevator_ShouldReturnClosestElevator_WhenAvailable()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 1, direction: ElevatorDirection.Up);

            List<Elevator> elevators = new List<Elevator>
        {
            CreateMockedElevator(1, 3, false), // Current floor 3, available
            CreateMockedElevator(2, 7, false), // Current floor 7, available
            CreateMockedElevator(3, 10, true)  // Current floor 10, busy
        };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(elevators, request);

            // Assert
            Assert.NotNull(allocatedElevator);
            Assert.AreEqual(1, allocatedElevator.ElevatorNumber); // Closest elevator is Elevator 1
        }

        [Test]
        public void AllocateElevator_ShouldReturnNull_WhenAllElevatorsAreBusy()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 1, direction: ElevatorDirection.Up);

            List<Elevator> elevators = new List<Elevator>
        {
            CreateMockedElevator(1, 3, true), // Current floor 3, busy
            CreateMockedElevator(2, 7, true), // Current floor 7, busy
            CreateMockedElevator(3, 10, true) // Current floor 10, busy
        };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(elevators, request);

            // Assert
            Assert.Null(allocatedElevator);
        }

        [Test]
        public void AllocateElevator_ShouldReturnNull_WhenNoElevatorsAreAvailable()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 1, direction: ElevatorDirection.Up);

            List<Elevator> elevators = new List<Elevator>(); // No elevators

            // Act
            var allocatedElevator = _strategy.AllocateElevator(elevators, request);

            // Assert
            Assert.Null(allocatedElevator);
        }

        [Test]
        public void AllocateElevator_ShouldReturnClosestElevator_WhenMultipleAvailable()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 1, direction: ElevatorDirection.Up);

            List<Elevator> elevators = new List<Elevator>
        {
            CreateMockedElevator(1, 1, false), // Current floor 1, available
            CreateMockedElevator(2, 7, false), // Current floor 6, available
            CreateMockedElevator(3, 4, false)  // Current floor 4, available
        };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(elevators, request);

            // Assert
            Assert.NotNull(allocatedElevator);
            Assert.AreEqual(3, allocatedElevator.ElevatorNumber); // Closest elevator is Elevator 3 (floor 4)
        }

        private Elevator CreateMockedElevator(int elevatorNumber, int currentFloor, bool isBusy)
        {
            var mockElevator = new Mock<Elevator>(elevatorNumber);
            mockElevator.Setup(e => e.CurrentFloor).Returns(currentFloor);
            mockElevator.Setup(e => e.IsBusy).Returns(isBusy);
            return mockElevator.Object;
        }
    }

}
