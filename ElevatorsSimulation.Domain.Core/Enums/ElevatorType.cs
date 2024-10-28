using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Enums
{
    public enum ElevatorType
    {
        [Description("Passenger Elevator")]
        Passenger = 1,
        [Description("Service Elevator")]
        Service = 2
    }
}
