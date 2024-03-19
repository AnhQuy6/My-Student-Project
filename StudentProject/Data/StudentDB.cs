using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using StudentProject.Data.Config;

namespace StudentProject.Data
{
    public class StudentDB : DbContext
    {
        public StudentDB(DbContextOptions<StudentDB> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
        public DbSet<Student> Students { get; set; }
    }
}
