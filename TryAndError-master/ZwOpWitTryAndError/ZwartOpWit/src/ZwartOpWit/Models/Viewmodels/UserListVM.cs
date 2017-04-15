using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class UserListVM
    {
        public List<User> userList { get; set; }

        public UserListVM()
        {
            userList = new List<User>();
        }
    }
}
