using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Models.Elevators
{
    public class PassengerElevator : Elevator
    {
        public int PassengerLimit { get; private set; }

        public PassengerElevator(int elevatorNumber, int passengerLimit) :
            base(elevatorNumber)
        {
            PassengerLimit = passengerLimit;
        }

        public override bool CapacityReached(double requestedCapacity)
        {
            if (requestedCapacity + CurrentLoad >= PassengerLimit)
                return true;

            return false;
        }

        public override bool Load(double loadAmount)
        {
            if (CurrentLoad + loadAmount > PassengerLimit)
            {
                // Log Message - Passenger Limit Exceeded
                return false;
            }

            if (Status == Enums.ElevatorStatus.Stationary)
            {
                CurrentLoad = CurrentLoad + loadAmount;
                return true;
            }

            // Log Message - Passengers can not be loaded when elevator Status is : ElevatorStatus
            return false;
        }

        public override bool Unload(double unloadAmount)
        {
            if (CurrentLoad - unloadAmount < 0)
            {
                // Log Message - Could not unload more than the current number of passenger : _currentNumberOfPassengers
                return false;
            }
            if (Status == Enums.ElevatorStatus.Stationary)
            {
                CurrentLoad = CurrentLoad - (int)unloadAmount;
                return true;
            }
            // Log Message - Passengers can only be unloaded when elevator Status is : ElevatorStatus
            return false;
        }
    }
}
