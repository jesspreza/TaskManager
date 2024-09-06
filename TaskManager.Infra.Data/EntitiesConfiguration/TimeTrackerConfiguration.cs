using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Data.EntitiesConfiguration
{
    public class TimeTrackerConfiguration : IEntityTypeConfiguration<TimeTracker>
    {
        public void Configure(EntityTypeBuilder<TimeTracker> builder)
        {
            builder
                .Property(tt => tt.TimeZoneId)
                .IsRequired()
                .HasMaxLength(200);
            builder
                .Property(tt => tt.CollaboratorId)
                .IsRequired();
        }
    }
}
