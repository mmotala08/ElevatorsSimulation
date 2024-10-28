namespace ElevatorsSimulation.BusinessLogic.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using ElevatorsSimulation.Domain.Core.Models.Elevators;
    using ElevatorsSimulation.Domain.Core.Enums;
    using ElevatorsSimulation.BusinessLogic.Logic.DispatchStrategies;
    using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
    using Moq;

    public class DirectionAwareStrategyTests
    {

        private readonly DirectionAwareStrategy _strategy;


        public DirectionAwareStrategyTests()
        {
            _strategy = new DirectionAwareStrategy();
        }

        [Test]
        public void AllocateElevator_ShouldReturnClosestElevatorWithSameDirection()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 3, direction: ElevatorDirection.Up);

            List<Elevator> _elevators = new List<Elevator>() {
            CreateMockedElevator(1, ElevatorDirection.Up, 1, false),
            CreateMockedElevator(2, ElevatorDirection.None, 4, false),
            CreateMockedElevator(3, ElevatorDirection.None, 10, false)
            };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(_elevators, request);

            // Assert
            Assert.NotNull(allocatedElevator);
            Assert.AreEqual(2, allocatedElevator.ElevatorNumber);
        }

        [Test]
        public void AllocateElevator_ShouldReturnIdleElevator_WhenNoElevatorMatchesDirection()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 8, requestCapacity: 1, direction: ElevatorDirection.Down);

            List<Elevator> _elevators = new List<Elevator>() {
            CreateMockedElevator(1, ElevatorDirection.Up, 1, false),
            CreateMockedElevator(2, ElevatorDirection.None, 5, false),
            CreateMockedElevator(3, ElevatorDirection.None, 10, false)
            };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(_elevators, request);

            // Assert
            Assert.NotNull(allocatedElevator);
            Assert.AreEqual(3, allocatedElevator.ElevatorNumber);
        }

        [Test]
        public void AllocateElevator_ShouldReturnNull_WhenAllElevatorsAreBusy()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 3, requestCapacity: 1, direction: ElevatorDirection.Up);

            List<Elevator> _elevators = new List<Elevator>() {
            CreateMockedElevator(1, ElevatorDirection.Up, 1, true),
            CreateMockedElevator(2, ElevatorDirection.None, 5, true),
            CreateMockedElevator(3, ElevatorDirection.None, 10, true)
                };

            // Act
            var allocatedElevator = _strategy.AllocateElevator(_elevators, request);

            // Assert
            Assert.Null(allocatedElevator);
        }

        private Elevator CreateMockedElevator(int elevatorNumber, ElevatorDirection direction, int currentFloor, bool isBusy)
        {
            var mockElevator = new Mock<Elevator>(elevatorNumber);
            mockElevator.Setup(e => e.Direction).Returns(direction);
            mockElevator.Setup(e => e.CurrentFloor).Returns(currentFloor);
            mockElevator.Setup(e => e.IsBusy).Returns(isBusy);
            return mockElevator.Object;
        }
    }
}
