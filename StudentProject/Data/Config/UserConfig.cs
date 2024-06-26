﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentProject.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.Username).IsRequired();
            builder.Property(n => n.Password).IsRequired();
            builder.Property(n => n.PasswordSalt).IsRequired();
            builder.Property(n => n.UserType).IsRequired();
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreateDate).IsRequired();
            builder.Property(n => n.ModifiedDate).IsRequired();
        }
    }
}
