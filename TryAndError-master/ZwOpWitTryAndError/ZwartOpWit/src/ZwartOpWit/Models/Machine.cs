using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class Machine
    {
        public Machine()
        {
            Stiches = new List<Stitch>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
        public  ICollection<Stitch> Stiches { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department{ get; set; }
    }
}
