using Attendace.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendace.Data
{
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{
		}
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Faculty> Faculties { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<FacultyCourse> FacultyCourses { get; set; }
		public DbSet<Routine> Routines { get; set; }
		public DbSet<QRSession> QRSessions { get; set; }
		public DbSet<AttendanceLog> AttendanceLogs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Unique constraints (optional but recommended)
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();

			modelBuilder.Entity<Student>()
				.HasIndex(s => s.RollNo)
				.IsUnique();

			// One-to-one relationships between User and Student/Faculty/Admin
			modelBuilder.Entity<User>()
				.HasOne(u => u.Student)
				.WithOne(s => s.User)
				.HasForeignKey<Student>(s => s.UserId);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Faculty)
				.WithOne(f => f.User)
				.HasForeignKey<Faculty>(f => f.UserId);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Admin)
				.WithOne(a => a.User)
				.HasForeignKey<Admin>(a => a.UserId);

			// Many-to-many relationship via FacultyCourse
			modelBuilder.Entity<FacultyCourse>()
	   .HasOne(fc => fc.Faculty)
	   .WithMany(f => f.FacultyCourses)
	   .HasForeignKey(fc => fc.FacultyId)
	   .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<FacultyCourse>()
	   .HasOne(fc => fc.Course)
	   .WithMany(c => c.FacultyCourses)
	   .HasForeignKey(fc => fc.CourseId)
	   .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Routine>()
	.HasOne(r => r.Faculty)
	.WithMany(f => f.Routines)
	.HasForeignKey(r => r.FacultyId)
	.OnDelete(DeleteBehavior.Restrict); // Prevent cascade from Faculty

			modelBuilder.Entity<Routine>()
				.HasOne(r => r.Course)
				.WithMany(c => c.Routines)
				.HasForeignKey(r => r.CourseId)
				.OnDelete(DeleteBehavior.Cascade); // Keep cascade from Course (or vice versa)

			modelBuilder.Entity<AttendanceLog>()
	.HasOne(al => al.QRSession)
	.WithMany(qr => qr.AttendanceLogs)
	.HasForeignKey(al => al.QRSessionId)
	.OnDelete(DeleteBehavior.Cascade); // keep cascade here

			modelBuilder.Entity<AttendanceLog>()
				.HasOne(al => al.Student)
				.WithMany(s => s.AttendanceLogs)
				.HasForeignKey(al => al.StudentId)
				.OnDelete(DeleteBehavior.Restrict); // disable cascade here

		}
	}
}
