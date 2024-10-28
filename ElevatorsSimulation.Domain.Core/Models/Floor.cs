using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorsSimulation.Domain.Core.Models
{
    public class Floor
    {
        public int FloorNumber { get; }

        public Floor(int floorNumber)
        {

            FloorNumber = floorNumber;
        }
    }

}
