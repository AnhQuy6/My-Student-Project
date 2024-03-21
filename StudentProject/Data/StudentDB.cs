using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using StudentProject.Data.Config;

namespace StudentProject.Data
{
    public class StudentDB : DbContext
    {
        public StudentDB(DbContextOptions<StudentDB> options):base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
        }
        
    }
}
