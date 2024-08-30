using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    public partial class Task : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; private set; }

        public string? Description { get; private set; }

        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; private set; }

        public virtual Project? Project { get; private set; }

        public virtual ICollection<TimeTracker> TimeTrackers { get; private set; } = new List<TimeTracker>();
    }
}
