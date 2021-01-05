using System;
using System.Collections.Generic;
using System.Text;

namespace GetInITCodingChallenge
{
    public class HardwareUnit
    {
        public string Name { get; set; }
        /// <summary>
        /// Weight in Kilogram
        /// </summary>
        public double Weight { get; set; }
        public uint Utility { get; set; }

        public uint Id { get; set; }

        public HardwareUnit(HardwareNeeded hwNeeded, uint id)
        {
            this.Name = hwNeeded.Name;
            this.Weight = hwNeeded.Weight;
            this.Utility = hwNeeded.Utility;
            this.Id = id;
        }
    }
}
