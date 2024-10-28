using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Abstractions
{
    public interface IElevatorDisplay
    {
        public void DisplayMessage(string message);
    }
}
