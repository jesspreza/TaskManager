using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.Entities
{
    public partial class TimeTracker : BaseEntity
    {
        [Required]
        public DateTime StartDate { get; private set; }

        [Required]
        public DateTime EndDate { get; private set; }

        [Required]
        [MaxLength(200)]
        public string TimeZoneId { get; private set; }
        
        [ForeignKey(nameof(Task))]
        [Required]
        public Guid TaskId { get; private set; }
        public virtual Task? Task { get; set; }
        
        [ForeignKey(nameof(Collaborator))]
        [Required]
        public Guid CollaboratorId { get; private set; }
        public virtual Collaborator? Collaborator { get; private set; }
    }
}
