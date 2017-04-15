using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class Stitch
    {
        public int Id { get; set; }
        public string JobNumber { get; set; }
        public int Quantity { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }
        public int PageQuantity { get; set; }
        public int Cover { get; set; }
        public string PaperBw { get; set; }
        public string PaperCover { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public String UserId { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
        public int MachineId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime StopDateTime { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
