using CSAS.Models;
using Microsoft.EntityFrameworkCore;

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
		public DbSet<ActivityTemplate> Templates { get; set; }
		public DbSet<TaskTemplate> TasksTemplate { get; set; }
		public DbSet<Settings> Settings { get; set; }
		public DbSet<FinalAssessment> FinalAssessment { get; set; }


	}
}
