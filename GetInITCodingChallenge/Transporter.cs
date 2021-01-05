using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetInITCodingChallenge
{
    public class Transporter
    {
        /// <summary>
        /// max. Capacity in kilogram
        /// </summary>
        public double Capacity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ContentWeight { get; set; }

        public uint UtilityTransporting { get; set; }

        private Driver driver;
        /// <summary>
        /// 
        /// </summary>
        public Driver Driver { get
            {
                return driver;
            }
            set
            {
                if(this.driver != null)
                {
                    this.ContentWeight -= this.driver.Weight;
                }
                this.ContentWeight += value.Weight;
                this.driver = value;
            }
        }

        private List<HardwareUnit> hardwareTransporting = new List<HardwareUnit>();
        public List<HardwareUnit> HardwareTransporting
        {
            get {
                return hardwareTransporting;
            }
        }

        public bool AddHardware(HardwareUnit hwUnit)
        {
            if((this.ContentWeight + hwUnit.Weight) <= this.Capacity)
            {
                this.UtilityTransporting += hwUnit.Utility;
                this.ContentWeight += hwUnit.Weight;
                this.hardwareTransporting.Add(hwUnit);
                return true;
            }
            else
            {
                //throw new InvalidOperationException($"transporter cannot add further hardware: {hardware.Name}because the weight: {hardware.Weight} does not fit into the remaining {this.Capacity - this.ContentWeight}");
                return false;
            }
        }
        public void WriteTransporterContent()
        {
            Console.WriteLine($"Transporter with Capacity: {Capacity+Environment.NewLine}is filled with driver that weighs: {driver.Weight + Environment.NewLine}and with hardware (+driver) up to Weight:{ContentWeight + Environment.NewLine}bringing in Utility: {UtilityTransporting + Environment.NewLine}with the following content:");
            List<string> hwNames = new List<string>();
            foreach(var unit in this.hardwareTransporting)
            {
                if (!hwNames.Contains(unit.Name))
                {
                    hwNames.Add(unit.Name);
                }
            }
            foreach(string hwName in hwNames)
            {
                IEnumerable<HardwareUnit> unitsWithName = hardwareTransporting.Where(el => el.Name == hwName);
                Console.WriteLine($"{hwName} : {unitsWithName.Count()}");
            }
            
        }
    }
}
