using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    public class TimeTrackerDto
    {
        public Guid? Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string TimeZoneId { get; set; }

        [Required]
        public Guid TaskId { get; set; }
        public virtual TaskDto? TaskDto { get; set; }

        public Guid? CollaboratorId { get; set; }
        public virtual CollaboratorDto? CollaboratorDto { get; set; }
    }
}
