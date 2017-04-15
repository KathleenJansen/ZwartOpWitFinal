using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models.Viewmodels
{
    public class DepartmentListVM
    {
        public List<Department> departmentList { get; set; }

        public DepartmentListVM()
        {
            departmentList = new List<Department>();
        }
    }
}
