using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Enums
{
    public enum ElevatorDirection
    {
        [Description("Up")]
        Up = 1,
        [Description("Down")]
        Down = 2,
        [Description("None")]
        None = 3
    }
}
