using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;


namespace ZwartOpWit.Models.Viewmodels
{
    public class DepartmentListVM : IndexBaseVM
    {
        public PaginatedList<Department> departmentList { get; set; }

        public DepartmentListVM()
        {
            departmentList = new PaginatedList<Department>();
        }
        public String nameSortParm { get; set; }
    }
}
