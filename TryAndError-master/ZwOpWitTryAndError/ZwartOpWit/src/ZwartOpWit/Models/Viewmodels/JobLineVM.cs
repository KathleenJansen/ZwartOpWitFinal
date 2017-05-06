using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class JobLineVM
    {
        public JobLine jobLine { get; set; }

        public string date { get; set; }

        public JobLineVM()
        {
            Job job = new Job();
        }
    }
}
