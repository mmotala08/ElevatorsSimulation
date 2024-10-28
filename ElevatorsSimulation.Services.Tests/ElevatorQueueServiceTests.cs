namespace ElevatorsSimulation.Services.Tests
{
    using ElevatorsSimulation.Domain.Core.Enums;
    using ElevatorsSimulation.Domain.Core.Models.Elevators.Requests;
    using ElevatorsSimulation.Services.Services;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ElevatorQueueServiceTests
    {
        private ElevatorQueueService _queueService;

        [SetUp]
        public void Setup()
        {
            _queueService = new ElevatorQueueService();
        }

        [Test]
        public async Task Enqueue_ShouldAddRequestToQueue()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 3, direction: ElevatorDirection.Up);

            // Act
            await _queueService.Enqueue(request);

            // Assert
            Assert.IsTrue(_queueService.HasRequests());
        }

        [Test]
        public void Dequeue_ShouldReturnRequest_WhenQueueHasRequests()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 3, direction: ElevatorDirection.Up);
            _queueService.Enqueue(request).Wait();

            // Act
            var dequeuedRequest = _queueService.Dequeue();

            // Assert
            Assert.AreEqual(request, dequeuedRequest);
        }

        [Test]
        public void Dequeue_ShouldThrowException_WhenQueueIsEmpty()
        {
            // Arrange
            // Ensure the queue is empty

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _queueService.Dequeue());
        }

        [Test]
        public void Peek_ShouldReturnRequest_WhenQueueHasRequests()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 3, direction: ElevatorDirection.Up);
            _queueService.Enqueue(request).Wait();

            // Act
            var peekedRequest = _queueService.Peek();

            // Assert
            Assert.AreEqual(request, peekedRequest);
        }

        [Test]
        public void Peek_ShouldThrowException_WhenQueueIsEmpty()
        {
            // Arrange
            // Ensure the queue is empty

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _queueService.Peek());
        }

        [Test]
        public void HasRequests_ShouldReturnFalse_WhenQueueIsEmpty()
        {
            // Arrange
            // Ensure the queue is empty

            // Act
            var hasRequests = _queueService.HasRequests();

            // Assert
            Assert.IsFalse(hasRequests);
        }

        [Test]
        public async Task HasRequests_ShouldReturnTrue_WhenQueueHasRequests()
        {
            // Arrange
            var request = new ElevatorFloorRequest(floor: 5, requestCapacity: 3, direction: ElevatorDirection.Up);

            // Act
            await _queueService.Enqueue(request);
            var hasRequests = _queueService.HasRequests();

            // Assert
            Assert.IsTrue(hasRequests);
        }
    }

}