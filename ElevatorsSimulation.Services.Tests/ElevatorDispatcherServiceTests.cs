using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ElevatorsSimulation.BusinessLogic.Abstractions;
using ElevatorsSimulation.Domain.Core.Abstractions;
using ElevatorsSimulation.Domain.Core.Enums;
using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
using ElevatorsSimulation.Domain.Core.Models.Elevators;
using ElevatorsSimulation.Services.Services.Abstractions;
using ElevatorsSimulation.Services.Services;

namespace ElevatorsSimulation.Services.Tests
{
    public class ElevatorDispatcherServiceTests
    {
        private readonly Mock<IBuilding> _buildingMock;
        private readonly Mock<IElevatorQueueService> _elevatorQueueServiceMock;
        private readonly Mock<IElevatorDispatchStrategy> _elevatorDispatchStrategyMock;
        private readonly Mock<IElevatorRequestPriorityStrategy> _elevatorRequestPriorityStrategyMock;
        private ElevatorDispatcherService _dispatcherService;

        public ElevatorDispatcherServiceTests()
        {
            _buildingMock = new Mock<IBuilding>();
            _elevatorQueueServiceMock = new Mock<IElevatorQueueService>();
            _elevatorDispatchStrategyMock = new Mock<IElevatorDispatchStrategy>();
            _elevatorRequestPriorityStrategyMock = new Mock<IElevatorRequestPriorityStrategy>();


        }

        [Test]
        public async Task HandleElevatorRequests_ShouldProcessRequest_WhenElevatorIsAvailable()
        {
            // Arrange
            var elevatorMock = new Mock<PassengerElevator>(1, 10);
            elevatorMock.Setup(x => x.IsBusy).Returns(false);
            var floorRequest = new ElevatorFloorRequest(5, 3, ElevatorDirection.Up);

            var hasRequestsCallCount = 0;
            _buildingMock.Setup(b => b.Elevators).Returns(new List<Elevator> { elevatorMock.Object });
            _elevatorQueueServiceMock.Setup(q => q.HasRequests()).Returns(() => hasRequestsCallCount++ <= 1);
            _elevatorQueueServiceMock.Setup(q => q.Peek()).Returns(floorRequest);
            _elevatorQueueServiceMock.Setup(q => q.Dequeue()).Returns(floorRequest);
            _elevatorDispatchStrategyMock.Setup(s => s.AllocateElevator(It.IsAny<List<Elevator>>(), floorRequest))
                .Returns(elevatorMock.Object);

            _dispatcherService = new ElevatorDispatcherService(
               _buildingMock.Object,
               _elevatorQueueServiceMock.Object,
               _elevatorDispatchStrategyMock.Object,
               _elevatorRequestPriorityStrategyMock.Object
           );

            // Act
            await _dispatcherService.HandleElevatorRequests();

            // Assert
            _elevatorQueueServiceMock.Verify(q => q.Dequeue(), Times.Once);
            Assert.True(elevatorMock.Object.IsDispatched);
        }

        [Test]
        public async Task HandleElevatorRequests_ShouldNotProcess_WhenNoAvailableElevators()
        {
            // Arrange
            var elevatorMock = new Mock<PassengerElevator>(1, 10);
            elevatorMock.Setup(x => x.IsBusy).Returns(true);
            var floorRequest = new ElevatorFloorRequest(5, 3, ElevatorDirection.Up);

            var hasRequestsCallCount = 0;
            _buildingMock.Setup(b => b.Elevators).Returns(new List<Elevator> { elevatorMock.Object });
            _elevatorQueueServiceMock.Setup(q => q.HasRequests()).Returns(() => hasRequestsCallCount++ <= 1);
            _elevatorQueueServiceMock.Setup(q => q.Peek()).Returns(floorRequest);
            _elevatorDispatchStrategyMock.Setup(s => s.AllocateElevator(It.IsAny<List<Elevator>>(), floorRequest))
                .Returns((Elevator?)null);

            _dispatcherService = new ElevatorDispatcherService(
               _buildingMock.Object,
               _elevatorQueueServiceMock.Object,
               _elevatorDispatchStrategyMock.Object,
               _elevatorRequestPriorityStrategyMock.Object
           );
            // Act
            await _dispatcherService.HandleElevatorRequests();

            // Assert
            _elevatorQueueServiceMock.Verify(q => q.Dequeue(), Times.Never);
        }

       
    }

}
