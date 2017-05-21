﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobListMachineVM : IndexBaseVM
    {
       // public List<Job> jobList { get; set; }

        public PaginatedList<JobLine> jobLineList { get; set; }

        public string filterDateTime { get; set; }

        public String jobNumberSortParm { get; set; }
        public String pageQuantitySortParm { get; set; }
        public String quantitySortParm { get; set; }
        public MachineTypes filterMachineType { get; set; }
        public int filterMachineId { get; set; }
        public TimeSpan totalTime { get; set; }

        public JobListMachineVM()
        {
            jobLineList = new PaginatedList<JobLine>();
            filterDateTime = DateTime.Today.ToString();
        }
    }
}
