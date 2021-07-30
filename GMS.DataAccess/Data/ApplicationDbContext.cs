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
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainer{ get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
     

    }
}
