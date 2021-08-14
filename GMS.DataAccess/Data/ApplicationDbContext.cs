using GMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gym_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Trainer> Trainer{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<TrainerSchedule> TrainerSchedule { get; set; }


    }
}
