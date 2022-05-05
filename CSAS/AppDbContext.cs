using CSAS.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;


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
			.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AssessmentDatabase.mdf;Integrated Security=True");

			//Offline SQL DB and Online SQL DB
			//#if !DEBUG
			//optionsBuilder.UseLazyLoadingProxies()
			//						.UseSqlServer("workstation id=CSAS-DB.mssql.somee.com;packet size=4096;user id=rarxxx_SQLLogin_1;pwd=b2koo97jka;data source=CSAS-DB.mssql.somee.com;persist security info=False;initial catalog=CSAS-DB ");

			//#else//#endif
			//optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ZZ03XZ693\Documents\CSASDatabase.mdf;Integrated Security=True;Connect Timeout=30");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Generating random GUID values as IDs
			modelBuilder.Entity<UserInfo>().HasKey(x => x.Id);
			modelBuilder.Entity<UserInfo>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Student>().HasKey(x => x.Id);
			modelBuilder.Entity<Student>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Activity>().HasKey(x => x.Id);
			modelBuilder.Entity<Activity>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<ActivityTemplate>().HasKey(x => x.Id);
			modelBuilder.Entity<ActivityTemplate>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Attachments>().HasKey(x => x.Id);
			modelBuilder.Entity<Attachments>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Attendance>().HasKey(x => x.Id);
			modelBuilder.Entity<Attendance>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<FinalAssessment>().HasKey(x => x.Id);
			modelBuilder.Entity<FinalAssessment>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<MainGroup>().HasKey(x => x.Id);
			modelBuilder.Entity<MainGroup>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Settings>().HasKey(x => x.Id);
			modelBuilder.Entity<Settings>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<SubAttendances>().HasKey(x => x.Id);
			modelBuilder.Entity<SubAttendances>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<SubGroup>().HasKey(x => x.Id);
			modelBuilder.Entity<SubGroup>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<Task>().HasKey(x => x.Id);
			modelBuilder.Entity<Task>().Property(x => x.Id).ValueGeneratedOnAdd();

			modelBuilder.Entity<TaskTemplate>().HasKey(x => x.Id);
			modelBuilder.Entity<TaskTemplate>().Property(x => x.Id).ValueGeneratedOnAdd();
		}
		public DbSet<Student> Students { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<Activity> Activities { get; set; }
		public DbSet<MainGroup> MainGroups { get; set; }
		public DbSet<SubGroup> SubGroups { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<ActivityTemplate> Templates { get; set; }
		public DbSet<TaskTemplate> TasksTemplate { get; set; }
		public DbSet<Settings> Settings { get; set; }
		public DbSet<FinalAssessment> FinalAssessment { get; set; }
		public DbSet<UserInfo> UserInfo { get; set; }


	}
}
