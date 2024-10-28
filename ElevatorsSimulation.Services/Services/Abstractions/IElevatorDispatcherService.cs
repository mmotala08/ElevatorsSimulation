using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Services.Services.Abstractions
{
    public interface IElevatorDispatcherService
    {
        Task HandleElevatorRequests();
    }
}
