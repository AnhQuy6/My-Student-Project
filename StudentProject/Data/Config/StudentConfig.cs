using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentProject.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Id).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(30);

            builder.HasData(new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Name = "Quy",
                    Phone = 0977572993,
                    Email = "hoangquy3125@gmail.com",
                    Address = "Ha Tinh"
                },
                new Student
                {
                    Id = 2,
                    Name = "Thao",
                    Phone = 0961346807,
                    Email = "vanthao23062003@gmail.com",
                    Address = "NinhBinh"
                },
                new Student
                {
                    Id = 3,
                    Name = "My",
                    Phone = 0977573661,
                    Email = "my@gmail.com",
                    Address = "Ha Noi"
                }
            });

            builder.HasOne(x => x.Department)
                .WithMany(n => n.Students)
                .HasForeignKey(n => n.DepartmentId)
                .HasConstraintName("FK_Students_Department");
        }
    }
}
