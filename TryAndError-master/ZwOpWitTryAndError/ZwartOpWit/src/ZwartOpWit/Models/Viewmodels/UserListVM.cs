using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers;

namespace ZwartOpWit.Models.Viewmodels
{
    public class UserListVM : IndexBaseVM
    {
        public PaginatedList<User> userList { get; set; }

        public UserListVM()
        {
            userList = new PaginatedList<User>();
        }

        public String EmailSortParm { get; set; }
    }
}
