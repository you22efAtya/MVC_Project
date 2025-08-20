﻿using Demo.DAL.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Data.Configurations.Departments
{
    internal class DepartmentConfugrations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(D => D.Code).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");//update and add
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");//add only
            builder.HasMany(D=>D.Employees)
                .WithOne(E => E.Department)
                .HasForeignKey(E => E.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
