namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing a Time Tracker in response operations.
    /// </summary>
    public class TimeTrackerResponseDto
    {
        /// <summary>
        /// Data Transfer Object for representing a Time Tracker in response operations.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the Time Tracker was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the Time Tracker was last updated. This is optional.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the Time Tracker was deleted. This is optional.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the Time Tracker entry.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the Time Tracker entry.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the time zone identifier for the Time Tracker entry.
        /// </summary>
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Task associated with the Time Tracker.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the Task details associated with the Time Tracker. This is optional.
        /// </summary>
        public virtual TaskResponseDto? TaskResponseDto { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Collaborator associated with the Time Tracker. This is optional.
        /// </summary>
        public Guid? CollaboratorId { get; set; }

        /// <summary>
        /// Gets or sets the Collaborator details associated with the Time Tracker. This is optional.
        /// </summary>
        public CollaboratorResponseDto? CollaboratorResponseDto { get; set; }
    }
}
