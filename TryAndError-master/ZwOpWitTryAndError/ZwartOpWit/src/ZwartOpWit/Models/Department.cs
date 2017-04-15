using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class Department
    {
        public Department()
        {
            Machines = new List<Machine>();    
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<Machine> Machines { get; set; }
    }
}
