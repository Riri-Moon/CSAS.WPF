using CSAS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;

namespace CSAS
{
    public class AppDbContext : DbContext
    {

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ZZ03XZ693\Documents\CSASDatabase.mdf;Integrated Security=True;Connect Timeout=30");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<MainGroup> MainGroups { get; set; }
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        //public DbSet<Student> Students { get; set; }


    }
}
