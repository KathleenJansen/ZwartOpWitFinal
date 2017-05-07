using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobListVM : IndexBaseVM
    {
       // public List<Job> jobList { get; set; }

        public PaginatedList<JobLine> jobLineList { get; set; }

        public List<Machine> machineList { get; set; }

        public string jobFilterDateTime { get; set; }

        public int jobId { get; set; }

        public int departmentId { get; set; }

        public int machineId { get; set; }

        public string machineName { get; set; }

        public TimeSpan totalTime { get; set; }

        public JobListVM()
        {
            //jobList = new List<Job>();
            jobLineList = new PaginatedList<JobLine>();
            jobFilterDateTime = DateTime.Today.ToString();
        }

        public String JobNumberSortParm { get; set; }
        public String PageQuantitySortParm { get; set; }
        public String QuantitySortParm { get; set; }
        public MachineTypes machineType { get; set; }
    }
}
