using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentProject.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.DepartmentID);
            builder.Property(x => x.DepartmentID).UseIdentityColumn();
            builder.Property(x => x.DepartmentID).IsRequired();
            builder.Property(x => x.DepartmentName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);

            builder.HasData(new List<Department>
            {
                new Department
                {
                    DepartmentID = 1,
                    DepartmentName = "A",
                    Description = "Software Testing",
                },
                new Department
                {
                    DepartmentID = 2,
                    DepartmentName = "B",
                    Description = "Helpdesk It"
                }
            });
            
        }
    }
}
