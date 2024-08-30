using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infra.Data.EntitiesConfiguration
{
    public class CollaboratorConfiguration : IEntityTypeConfiguration<Collaborator>
    {
        public void Configure(EntityTypeBuilder<Collaborator> builder)
        {
            builder
                .HasMany(c => c.TimeTrackers)
                .WithOne(t => t.Collaborator)
                .HasForeignKey(t => t.CollaboratorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Collaborator>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}
