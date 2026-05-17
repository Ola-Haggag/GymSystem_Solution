using GymSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.Configurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("Varchar").HasMaxLength(50);
            
            builder.Property(p => p.Description).HasMaxLength(200);
            
            builder.Property(p => p.Price).HasPrecision(10, 2);// decimal 10,2
           
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GetDate()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheckValue" , "Duration Between 1 and 365");
            });
        } 
    }
}
