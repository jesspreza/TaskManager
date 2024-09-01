using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    public class TimeTrackerRequestDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Local time zone is required")]
        public string TimeZoneId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "A task must be selected")]
        public Guid TaskId { get; set; }

        public Guid? CollaboratorId { get; set; }
    }
}