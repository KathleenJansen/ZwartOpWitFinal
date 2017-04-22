using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class StartStop
    {
        public int Id { get; set; }
        public int JobNumber { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}
