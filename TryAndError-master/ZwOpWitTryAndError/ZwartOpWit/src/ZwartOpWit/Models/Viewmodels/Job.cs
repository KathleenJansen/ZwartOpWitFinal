using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class Job
    {
        public Job()
        {
            JobLineList = new List<JobLine>();
        }
        public int Id { get; set; }
        public string JobNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }
        public int PageQuantity { get; set; }
        public int Cover { get; set; }
        public string PaperBw { get; set; }
        public string PaperCover { get; set; }
        public List<JobLine> JobLineList { get; set; }
    }
}
