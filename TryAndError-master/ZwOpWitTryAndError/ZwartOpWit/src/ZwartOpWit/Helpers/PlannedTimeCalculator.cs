using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;

namespace ZwartOpWit.Helpers
{
    public class PlannedTimeCalculator
    {
        private readonly AppDBContext _context;

        public PlannedTimeCalculator(AppDBContext context)
        {
            _context = context;
        }

        public TimeSpan CalculatePlannedTimeStich(JobLine line)
        {
            Machine machine = _context.Machines.Where(m => m.Id == line.MachineId).FirstOrDefault();

            int stations = line.Job.PageQuantity / 4;
            int quantity = line.Job.Quantity;

            double instelMachine = machine.SetupTime + (stations * machine.SetupTimeStationFactor);
            double startDraai1000 = machine.RunTimeTo1000Speed + (stations * machine.RunTimeTo1000SpeedStationFactor);
            double doorDraai1000 = machine.RunTimeFrom1000Speed + (stations * machine.RunTimeFrom1000SpeedStationFactor);
            double quantityDoorDraai = quantity - 1000;
            double centiTime = instelMachine + startDraai1000 + (doorDraai1000 * quantityDoorDraai / 1000);

            //double instelMachine = 0.18 + (stations * 0.02);
            //double startDraai1000 = 0.268 + (stations * 0.0224);
            //double doorDraai1000 = 0.241 + (stations * 0.0163);
            //double quantityDoorDraai = quantity - 1000;
            //double centiTime = instelMachine + startDraai1000 + (doorDraai1000 * quantityDoorDraai / 1000);

            int hour = 0;
            int minute = 0;
            int second = 0;

            if (centiTime >= 1)
            {
                hour = 1;
                centiTime--;
            }

            double centiMinute = centiTime % 1;
            minute = (int)Math.Round(centiMinute * 60);

            double centiSecond = (centiMinute * 60) - minute;
            second = (int)Math.Round(centiSecond * 60);

            TimeSpan planned = new TimeSpan(hour, minute, second);
            return planned;
        }
    }
}
