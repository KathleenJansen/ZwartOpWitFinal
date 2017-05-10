using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Models.Viewmodels
{
    public class MachineListVM : IndexBaseVM
    {
        public PaginatedList<Machine> machineList { get; set; }

        public MachineListVM()
        {
            machineList = new PaginatedList<Machine>();
        }

        public String nameSortParm { get; set; }
        public String calculationMethodSortParm { get; set; }
        public String typeSortParm { get; set; }
        public String departmentSortParm { get; set; }
    }
}
