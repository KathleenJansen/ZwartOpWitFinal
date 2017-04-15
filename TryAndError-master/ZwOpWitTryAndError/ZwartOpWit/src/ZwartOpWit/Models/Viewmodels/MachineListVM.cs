using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class MachineListVM
    {
        public List<Machine> machineList { get; set; }

        public MachineListVM()
        {
            machineList = new List<Machine>();
        }
    }
}
