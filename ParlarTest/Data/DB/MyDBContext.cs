using Microsoft.EntityFrameworkCore;
using ParlarTest.Data.Entity;
using ParlarTest.Entity.Models;

namespace ParlarTest.Data.DB
{
    public class MyDBContext : DbContext
    {
        //public DbSet<StudentLesson> StudentLessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }


        public string DbPath { get; set; }

        public MyDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .HasPrincipalKey<User>(u => u.Id);
            
            modelBuilder.Entity<Student>()
                .HasMany(e => e.Leesons)
                .WithMany(e => e.Students)
                .UsingEntity(
                    "PostTag",
                    l => l.HasOne(typeof(Lesson)).WithMany().HasForeignKey("LessonsId").HasPrincipalKey(nameof(Lesson.Id)),
                    r => r.HasOne(typeof(Student)).WithMany().HasForeignKey("StudentsId").HasPrincipalKey(nameof(Student.Id)),
                    j => j.HasKey("StudentsId", "LessonsId"));

            
            // modelBuilder.Entity<StudentLesson>()
            //     .HasKey(sl => new { sl.StudentId, sl.LessonId });
            //
            // modelBuilder.Entity<StudentLesson>()
            //     .HasOne(sl => sl.Student)
            //     .WithMany(s => s.StudentLessons)
            //     .HasForeignKey(sl => sl.StudentId);
            //
            //
            // modelBuilder.Entity<StudentLesson>()
            //     .HasOne(sl => sl.Lesson)
            //     .WithMany(l => l.StudentLessons)
            //     .HasForeignKey(sl => sl.LessonId);

            modelBuilder.Entity<User>().HasIndex(e => e.UserName).IsUnique();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}