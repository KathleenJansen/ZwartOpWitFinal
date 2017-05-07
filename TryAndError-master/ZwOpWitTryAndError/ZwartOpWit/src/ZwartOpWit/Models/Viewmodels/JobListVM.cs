using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobListVM
    {
        public List<Job> jobList { get; set; }

        public List<JobLine> jobLineList { get; set; }

        public List<Machine> machineList { get; set; }

        public string date { get; set; }

        public int jobId { get; set; }

        public int departmentId { get; set; }

        public int machineId { get; set; }

        public string machineName { get; set; }

        public TimeSpan totalTime { get; set; }

        public JobListVM()
        {
            jobList = new List<Job>();
            jobLineList = new List<JobLine>();
            date = DateTime.Today.ToString();
        }
    }
}
