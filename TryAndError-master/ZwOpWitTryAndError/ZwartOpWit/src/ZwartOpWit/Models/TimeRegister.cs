﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZwartOpWit.Models
{
    public class TimeRegister
    {
        public int Id { get; set; }
        [ForeignKey("JobLineId")]
        public JobLine JobLine{ get; set; }
        public int JobLineId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public String UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}