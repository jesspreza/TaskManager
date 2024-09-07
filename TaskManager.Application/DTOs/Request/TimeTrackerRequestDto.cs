using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object for Time Tracker creation and update requests.
    /// </summary>
    public class TimeTrackerRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Time Tracker.
        /// This is optional during creation, but required for updates.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the time zone ID for the Time Tracker.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Local time zone is required")]
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets the start date and time for the time tracking.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date and time for the time tracking.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated Task.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "A task must be selected")]
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated Collaborator.
        /// This field is optional.
        /// </summary>
        public Guid CollaboratorId { get; set; }
    }
}