using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class JobLineCalculatedTime : JobLine
    {
        public TimeSpan CalculatedTime { get; set; }
    }
}
