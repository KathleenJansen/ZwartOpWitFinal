using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZwartOpWit.Helpers.CalculationMethods;
using static ZwartOpWit.Models.Machine;

namespace ZwartOpWit.Models
{

    public class JobLine
    {
        public int Id { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
        public int? MachineId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }
        public int JobId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string UserId { get; set; }
        public MachineTypes MachineType { get; set; }
        public int Sequence { get; set; }
        public bool Completed { get; set; }
        public TimeSpan CalculatedTime { get; set; }

        public void calculateTime(Machine machine)
        {
			ICalculationMethod calcMethod = null;

            if (machine != null)
            {
                this.Machine = machine;

                switch (machine.CalculationMethod)
                {
                    case CalculationMethodTypes.StichMain:
						calcMethod = new StichMainCalculationMethod();
                        break;
                    case CalculationMethodTypes.Always1Hour:
						calcMethod = new Always1Hour();
                        break;
                    case CalculationMethodTypes.Always30Min:
						calcMethod = new Always30Min();

						break;
                }

				if (calcMethod != null)
				{
					CalculatedTime = calcMethod.calculacte(this);
				}
			}
        }
    }
}
