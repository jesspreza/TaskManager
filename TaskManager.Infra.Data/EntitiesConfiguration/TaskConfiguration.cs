using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infra.Data.EntitiesConfiguration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder
                .HasMany(t => t.TimeTrackers)
                .WithOne(tt => tt.Task)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}
