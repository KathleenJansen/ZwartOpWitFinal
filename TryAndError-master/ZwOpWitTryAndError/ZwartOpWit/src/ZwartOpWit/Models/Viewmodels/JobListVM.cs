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

        public string date { get; set; }

        public int JobId { get; set; }

        public JobListVM()
        {
            jobList = new List<Job>();
            jobLineList = new List<JobLine>();
            date = DateTime.Today.ToString();
        }
    }
}
