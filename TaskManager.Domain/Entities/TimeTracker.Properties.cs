using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Guid TaskId { get; private set; }
        public virtual Task? Task { get; set; }
        
        [ForeignKey(nameof(Collaborator))]
        public Guid? CollaboratorId { get; private set; }
        public virtual Collaborator? Collaborator { get; private set; }
    }
}
