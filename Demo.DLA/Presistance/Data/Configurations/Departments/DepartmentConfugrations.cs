using Demo.DLA.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DLA.Presistance.Data.Configurations.Departments
{
    internal class DepartmentConfugrations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Name).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(D => D.Code).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");//update and add
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");//add only
        }
    }
}
