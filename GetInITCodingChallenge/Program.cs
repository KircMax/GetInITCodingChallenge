using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

/**
 * Author:    Max Kirchberger
 * Created:   05.01.2021
 * **/

namespace GetInITCodingChallenge
{
    
    public class Program
    {
        public static DirectoryInfo CurrentExeDir
        {
            get
            {
                string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                return (new FileInfo(dllPath)).Directory;
            }
        }

        /// <summary>
        /// I know the algorithm is not yet perfect since the transporters are not (yet) perfectly filled with low-weight but for this scenario this was by far not necessary
        /// and would only complicate the program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string HwNeededPath = Path.Combine(CurrentExeDir.FullName, "HWNeeded");
            string DriversPath = Path.Combine(CurrentExeDir.FullName, "Drivers");
            string TransportersPath = Path.Combine(CurrentExeDir.FullName, "Transporters");

            List<Transporter> transporters = new List<Transporter>();
            List<Driver> drivers = new List<Driver>();
            List<HardwareNeeded> hwNeeded = new List<HardwareNeeded>();
            foreach(var transporterPath in Directory.GetFiles(TransportersPath))
            {
                string transporterFileContent = File.ReadAllText(transporterPath);
                Transporter transporter = JsonConvert.DeserializeObject<Transporter>(transporterFileContent);
                transporters.Add(transporter);
            }
            foreach (var driverPath in Directory.GetFiles(DriversPath))
            {
                string driverFileContent = File.ReadAllText(driverPath);
                Driver driver = JsonConvert.DeserializeObject<Driver>(driverFileContent);
                drivers.Add(driver);
            }
            foreach (var hardwarePath in Directory.GetFiles(HwNeededPath))
            {
                string hardwareFileContent = File.ReadAllText(hardwarePath);
                HardwareNeeded hardwareNeeded = JsonConvert.DeserializeObject<HardwareNeeded>(hardwareFileContent);
                hardwareNeeded.CalculateUtilityPerKilogram();
                hwNeeded.Add(hardwareNeeded);
            }
            hwNeeded = hwNeeded.OrderByDescending(el => el.UtilityPerKilogram).ToList();
            if(transporters.Count != drivers.Count)
            {
                // if you wanted to do something in case you dont have the same amount of drivers as transporters!
            }
            int counter = 0;
            foreach(Transporter transporter in transporters)
            {
                transporter.Driver = drivers[counter];
                counter++;
            }
            foreach(Transporter transporter in transporters)
            {
                var filledWith = FillTransporter(transporter, hwNeeded);
                foreach(var hw in hwNeeded)
                {
                    var hwsAdded = filledWith.Where(el => el.Name == hw.Name);
                    hw.AmountNeeded -= (uint)hwsAdded.Count();
                    if(hw.AmountNeeded < 0)
                    {
                        throw new Exception("error in Program logic!");
                    }
                    else if(hw.AmountNeeded == 0)
                    {
                        // completely filled - nothing to do about it - could remove from list but not necessary!
                    }
                    else
                    {
                        //still hw of this kind can be needed in other location!
                    }
                }
            }
            foreach(Transporter transporter in transporters)
            {
                transporter.WriteTransporterContent();
            }
        }
        private static List<HardwareUnit> FillTransporter(Transporter transporter, List<HardwareNeeded> hardwareToFillTransporterWithOrderedByUtilityPerKilogram)
        {
            List<HardwareUnit> hardwareAddedToTransporter = new List<HardwareUnit>();
            uint hardwareCounter = 0;
            foreach(HardwareNeeded hardware in hardwareToFillTransporterWithOrderedByUtilityPerKilogram)
            {
                while(hardwareCounter < hardware.AmountNeeded)
                {
                    var hwUnit = new HardwareUnit(hardware, hardwareCounter);
                    if (transporter.AddHardware(hwUnit))
                    {
                        hardwareAddedToTransporter.Add(hwUnit);
                        hardwareCounter++;
                    }
                    else
                    {
                        // does not fit into transporter anymore - maybe we could still add something that has the next biggest utilityperKilogram but weighs less - there for just break and not return
                        break;
                    }
                }
                hardwareCounter = 0;
            }
            return hardwareAddedToTransporter;
        }
    }
}
