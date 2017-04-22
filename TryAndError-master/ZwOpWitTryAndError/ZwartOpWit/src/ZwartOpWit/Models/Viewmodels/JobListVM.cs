using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobListVM
    {
        public List<Job> jobList { get; set; }

        public String date { get; set; }

        public int JobId { get; set; }

        public JobListVM()
        {
            jobList = new List<Job>();
            date = DateTime.Today.ToString();
        }
    }
}
