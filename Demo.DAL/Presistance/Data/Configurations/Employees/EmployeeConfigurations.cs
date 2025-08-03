using Demo.DAL.Entities.Common.Enums;
using Demo.DAL.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Data.Configurations.Employees
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name)
            .HasColumnType("varchar(50)")
            .IsRequired();

            builder.Property(e => e.Address)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.Salary)
                .HasColumnType("decimal(8,2)");

            builder.Property(e => e.Email)
                .HasColumnType("varchar(100)");

            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");//update and add
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");//add only

            builder.Property(e => e.Gender)
                .HasConversion(
                    gender => gender.ToString(),
                    gender => (Gender)Enum.Parse(typeof(Gender), gender)
                );

            builder.Property(e => e.EmployeeType)
                .HasConversion(
                    employee => employee.ToString(),
                    employee => (EmployeeType)Enum.Parse(typeof(EmployeeType), employee)
                );
        }
    }
}
