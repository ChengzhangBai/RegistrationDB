using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Final.Models.DataAccess
{
    public partial class RegistrationDBContext : DbContext
    {
        public RegistrationDBContext()
        {
        }

        public RegistrationDBContext(DbContextOptions<RegistrationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Registration> Registration { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                IConfiguration Configuration = new ConfigurationBuilder()
                                    .SetBasePath(AppContext.BaseDirectory)
                                    .AddJsonFile("appsettings.json").Build();

                string connectionString = Configuration.GetConnectionString("RegistrationDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId)
                    .HasColumnName("CourseID")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.CourseTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.FeeBase).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => new { e.CourseCourseId, e.StudentStudentNum })
                    .HasName("PK__Registra__92ECCCE92AE4E752");

                entity.Property(e => e.CourseCourseId)
                    .HasColumnName("Course_CourseID")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.StudentStudentNum)
                    .HasColumnName("Student_StudentNum")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.CourseCourse)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.CourseCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToCourse");

                entity.HasOne(d => d.StudentStudentNumNavigation)
                    .WithMany(p => p.Registration)
                    .HasForeignKey(d => d.StudentStudentNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Registration_ToStudent");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentNum)
                    .HasName("PK__Student__B98EFCB567F3C9C7");

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
