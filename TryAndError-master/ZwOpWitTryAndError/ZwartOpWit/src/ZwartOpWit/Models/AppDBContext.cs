using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MySQL.Data.Entity.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ZwartOpWit.Models
{
    /// <summary>
    /// The entity framework context with a Employees DbSet
    /// </summary>
    public class AppDBContext : IdentityDbContext<User>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobLine> JobLines { get; set; }
        public DbSet<TimeRegister> TimeRegisters{ get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Job>().HasKey(m => m.Id);
            builder.Entity<Machine>().HasKey(m => m.Id);
            builder.Entity<Department>().HasKey(m => m.Id);
            builder.Entity<JobLine>().HasKey(m => m.Id);
            builder.Entity<TimeRegister>().HasKey(m => m.Id);
        }
    }
}
