using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Enums
{
    public enum ElevatorStatus
    {
        [Description("Stationary")]
        Stationary = 1,
        [Description("In Motion")]
        InMotion = 2,
        [Description("Out Of Service")]
        OutOfService = 3
    }
}
