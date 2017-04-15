using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class StitchJobsListVM
    {
        public List<Stitch> stitchJobsList { get; set; }

        public StitchJobsListVM()
        {
            stitchJobsList = new List<Stitch>();
        }
    }
}
