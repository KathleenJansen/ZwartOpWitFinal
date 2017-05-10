using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;


namespace ZwartOpWit.Models.Viewmodels
{
    public class TimeRegistertListVM : IndexBaseVM
    {
        public PaginatedList<TimeRegister> timeRegisterList { get; set; }

        public TimeRegistertListVM()
        {
            timeRegisterList = new PaginatedList<TimeRegister>();
        }
        public String jobIdSortParm { get; set; }
        public String userNameSortParm { get; set; }
        public String startTimeSortParm { get; set; }
        public String stopTimeSortParm { get; set; }
    }
}
