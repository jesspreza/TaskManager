using System.ComponentModel.DataAnnotations;

namespace TaskManager.Domain.Entities
{
    public partial class Collaborator : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; private set; }

        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public virtual ICollection<TimeTracker> TimeTrackers { get; private set; } = new List<TimeTracker>();
    }
}
