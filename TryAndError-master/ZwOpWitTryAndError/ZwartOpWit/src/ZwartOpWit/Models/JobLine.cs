using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ZwartOpWit.Models.Machine;

namespace ZwartOpWit.Models
{

    public class JobLine
    {
        public int Id { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
        public int MachineId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }
        public int JobId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string UserId { get; set; }
        public MachineTypes MachineType { get; set; }
        //public JobLineTypes JobLineType { get; set; }
        public int Sequence { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public bool Completed { get; set; }
        public TimeSpan CalculatedTime { get; set; }
    }
}
