using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;

namespace ZwartOpWit.Helpers.CalculationMethods
{
    public class Always1Hour : ICalculationMethod
    {
        public TimeSpan calculacte(JobLine jobLine)
        {
            return new TimeSpan(1,0,0);
        }
    }
}
