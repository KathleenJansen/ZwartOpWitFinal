using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobListToDoVM : IndexBaseVM
    {
        public PaginatedList<JobLine> jobLineList { get; set; }


        public string filterDateTime { get; set; }

        public String jobNumberSortParm { get; set; }
        public String pageQuantitySortParm { get; set; }
        public String quantitySortParm { get; set; }
        public MachineTypes filterMachineType { get; set; }
        public int filterMachineId { get; set; }

        public JobListToDoVM()
        {
            jobLineList = new PaginatedList<JobLine>();
            filterDateTime = DateTime.Today.ToString();
        }

        public int jobId { get; set; }
    }
}
