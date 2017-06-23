using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;

namespace ZwartOpWit.Helpers.CalculationMethods
{
    public class Always30Min : ICalculationMethod
    {
        public TimeSpan calculacte(JobLine jobLine)
        {
            return new TimeSpan(0, 30,0);
        }
    }
}
