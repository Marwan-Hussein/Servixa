using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models;

namespace Servixa.Presistence.Configurations
{
    public class WorkerTaskConfiguration : IEntityTypeConfiguration<WorkerTask>
    {
        public void Configure(EntityTypeBuilder<WorkerTask> builder)
        {
            builder.ToTable("WorkerTask");
            builder.HasKey(wt => wt.Id);
            builder.Property(wt => wt.CustomPrice).HasColumnType("decimal(18,2)");

            builder.HasOne(wt => wt.Worker)
                   .WithMany(w => w.WorkerTasks)
                   .HasForeignKey(wt => wt.WorkerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wt => wt.Task)
                   .WithMany(t => t.WorkerTasks)
                   .HasForeignKey(wt => wt.TaskId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
