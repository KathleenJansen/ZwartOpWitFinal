using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Models;

namespace ZwartOpWit.Helpers.CalculationMethods
{
    interface ICalculationMethod
    {
        TimeSpan calculacte(JobLine jobLine);
    }
}
