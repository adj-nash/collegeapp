using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig: IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired(false).HasMaxLength(250);

            builder.HasData(new List<Student>()
            {
                new Student
                { 
                    Id = 1, 
                    StudentName = "Alexander Nash", 
                    Address = "Nottingham", 
                    DOB = new DateTime(1990, 09, 05), 
                    Email="alex_nash@live.co.uk" }
                });
        }
    }
}
