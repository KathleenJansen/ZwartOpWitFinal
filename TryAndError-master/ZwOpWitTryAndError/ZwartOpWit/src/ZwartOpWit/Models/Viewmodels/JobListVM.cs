﻿using System;
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

        public string filterDateTime { get; set; }

        public String jobNumberSortParm { get; set; }
        public String pageQuantitySortParm { get; set; }
        public String quantitySortParm { get; set; }
        public MachineTypes filterMachineType { get; set; }
        public int filterMachineId { get; set; }
        public TimeSpan totalTime { get; set; }

        public JobListVM()
        {
            //jobList = new List<Job>();
            jobLineList = new PaginatedList<JobLine>();
            filterDateTime = DateTime.Today.ToString();
        }

        public int jobId { get; set; }

      
    }
}
