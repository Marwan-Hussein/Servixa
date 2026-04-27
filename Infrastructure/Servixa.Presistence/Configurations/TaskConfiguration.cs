using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Servixa.Presistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Servixa.Domain.Models.TaskEntity.Task>
    {
        public void Configure(EntityTypeBuilder<Servixa.Domain.Models.TaskEntity.Task> builder)
        {
            builder.ToTable("Task");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.AvgCost).HasColumnType("decimal(18,2)");

            builder.HasOne(t => t.Specialty)
                   .WithMany(s => s.Tasks)
                   .HasForeignKey(t => t.SpecialtyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
