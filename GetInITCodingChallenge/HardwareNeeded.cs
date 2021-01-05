using System;
using System.Collections.Generic;
using System.Text;

namespace GetInITCodingChallenge
{
    public class HardwareNeeded
    {
        public string Name { get; set; }
        public uint AmountNeeded { get; set; }
        private double weight;
        /// <summary>
        /// Weight in Kilogram
        /// </summary>
        public double Weight { get {
                return weight;
            }
            set
            {
                //unify units to always match kilogram!
                weight = value / 1000;
            }
        }
        public uint Utility { get; set; }

        public double UtilityPerKilogram;

        public void CalculateUtilityPerKilogram()
        {
            this.UtilityPerKilogram = this.Utility / this.Weight;
        }
    }
}
