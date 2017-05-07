using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public enum MachineTypes { Stitch, Typo, Fold, Score, Busch };
    public enum CalculationMethodTypes { None, StichMain, Always5000Ticks, Always1Hour }

    public class Machine
    {
        public Machine()
        {
            JobLines = new List<JobLine>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }
        public  ICollection<JobLine> JobLines { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department{ get; set; }
        public MachineTypes Type { get; set; }
        public Double SetupTime { get; set; }
        public Double SetupTimeStationFactor { get; set; }
        public Double RunTimeTo1000Speed { get; set; }
        public Double RunTimeTo1000SpeedStationFactor { get; set; }
        public Double RunTimeFrom1000Speed { get; set; }
        public Double RunTimeFrom1000SpeedStationFactor { get; set; }
        public CalculationMethodTypes CalculationMethod { get; set; }
    }
}
