// <auto-generated />
using System;
using CSAS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CSAS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220205172206_siganturePath")]
    partial class siganturePath
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CSAS.Models.Activity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsNotifyMe")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSendEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSendNotifications")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("CSAS.Models.ActivityTemplate", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("CSAS.Models.Attachments", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PathToFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("CSAS.Models.Attendance", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Form")
                        .HasColumnType("int");

                    b.Property<string>("MainGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("StudyForm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MainGroupId");

                    b.HasIndex("SubGroupId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("CSAS.Models.FinalAssessment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<bool>("IsNew")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSendAttendanceExport")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSendEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSendExport")
                        .HasColumnType("bit");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("FinalAssessment");
                });

            modelBuilder.Entity("CSAS.Models.MainGroup", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToFolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MainGroups");
                });

            modelBuilder.Entity("CSAS.Models.Settings", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("A")
                        .HasColumnType("int");

                    b.Property<int>("B")
                        .HasColumnType("int");

                    b.Property<int>("C")
                        .HasColumnType("int");

                    b.Property<int>("D")
                        .HasColumnType("int");

                    b.Property<int>("E")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("MaxPoints")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Signature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TitleAfterName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MainGroupId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("CSAS.Models.Student", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Form")
                        .HasColumnType("int");

                    b.Property<bool>("IndividualStudy")
                        .HasColumnType("bit");

                    b.Property<string>("Isic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToFolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubGroupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year")
                        .HasMaxLength(1)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MainGroupId");

                    b.HasIndex("SubGroupId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("CSAS.Models.SubAttendances", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AttendanceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AttendanceId");

                    b.HasIndex("StudentId");

                    b.ToTable("SubAttendances");
                });

            modelBuilder.Entity("CSAS.Models.SubGroup", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MainGroupId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathToFolder")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MainGroupId");

                    b.ToTable("SubGroups");
                });

            modelBuilder.Entity("CSAS.Models.Task", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("MaxPoints")
                        .HasMaxLength(3)
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Points")
                        .HasMaxLength(3)
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CSAS.Models.TaskTemplate", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActivityTemplateId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("MaxPoints")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActivityTemplateId");

                    b.ToTable("TasksTemplate");
                });

            modelBuilder.Entity("CSAS.Models.UserInfo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MachineName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("as")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("countryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("isp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("lat")
                        .HasColumnType("float");

                    b.Property<double>("lon")
                        .HasColumnType("float");

                    b.Property<string>("org")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("query")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("regionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("timezone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("zip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("CSAS.Models.Activity", b =>
                {
                    b.HasOne("CSAS.Models.Student", "Student")
                        .WithMany("ListOfActivities")
                        .HasForeignKey("StudentId");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CSAS.Models.Attachments", b =>
                {
                    b.HasOne("CSAS.Models.Activity", "Activity")
                        .WithMany("Attachments")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("CSAS.Models.Attendance", b =>
                {
                    b.HasOne("CSAS.Models.MainGroup", "MainGroup")
                        .WithMany()
                        .HasForeignKey("MainGroupId");

                    b.HasOne("CSAS.Models.SubGroup", "SubGroup")
                        .WithMany()
                        .HasForeignKey("SubGroupId");

                    b.Navigation("MainGroup");

                    b.Navigation("SubGroup");
                });

            modelBuilder.Entity("CSAS.Models.FinalAssessment", b =>
                {
                    b.HasOne("CSAS.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CSAS.Models.Settings", b =>
                {
                    b.HasOne("CSAS.Models.MainGroup", "MainGroup")
                        .WithMany()
                        .HasForeignKey("MainGroupId");

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("CSAS.Models.Student", b =>
                {
                    b.HasOne("CSAS.Models.MainGroup", "MainGroup")
                        .WithMany()
                        .HasForeignKey("MainGroupId");

                    b.HasOne("CSAS.Models.SubGroup", "SubGroup")
                        .WithMany("Students")
                        .HasForeignKey("SubGroupId");

                    b.Navigation("MainGroup");

                    b.Navigation("SubGroup");
                });

            modelBuilder.Entity("CSAS.Models.SubAttendances", b =>
                {
                    b.HasOne("CSAS.Models.Attendance", "Attendance")
                        .WithMany("SubAttendances")
                        .HasForeignKey("AttendanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CSAS.Models.Student", "Student")
                        .WithMany("SubAttendances")
                        .HasForeignKey("StudentId");

                    b.Navigation("Attendance");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CSAS.Models.SubGroup", b =>
                {
                    b.HasOne("CSAS.Models.MainGroup", "MainGroup")
                        .WithMany("SubGroups")
                        .HasForeignKey("MainGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("CSAS.Models.Task", b =>
                {
                    b.HasOne("CSAS.Models.Activity", "Activity")
                        .WithMany("Tasks")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("CSAS.Models.TaskTemplate", b =>
                {
                    b.HasOne("CSAS.Models.ActivityTemplate", "ActivityTemplate")
                        .WithMany("TasksTemplate")
                        .HasForeignKey("ActivityTemplateId");

                    b.Navigation("ActivityTemplate");
                });

            modelBuilder.Entity("CSAS.Models.Activity", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("CSAS.Models.ActivityTemplate", b =>
                {
                    b.Navigation("TasksTemplate");
                });

            modelBuilder.Entity("CSAS.Models.Attendance", b =>
                {
                    b.Navigation("SubAttendances");
                });

            modelBuilder.Entity("CSAS.Models.MainGroup", b =>
                {
                    b.Navigation("SubGroups");
                });

            modelBuilder.Entity("CSAS.Models.Student", b =>
                {
                    b.Navigation("ListOfActivities");

                    b.Navigation("SubAttendances");
                });

            modelBuilder.Entity("CSAS.Models.SubGroup", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
