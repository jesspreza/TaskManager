namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing a Time Tracker report in response operations.
    /// </summary>
    public class TimeTrackerReportResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Time Tracker.
        /// </summary>
        public Guid TimeTrackerId { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the Task associated with the Time Tracker.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Collaborator associated with the Time Tracker.
        /// </summary>
        public Guid CollaboratorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Collaborator. This is optional.
        /// </summary>
        public string? CollaboratorName { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the Time Tracker entry. This is optional.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the Time Tracker entry. This is optional.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the total time tracked per day per Collaborator. This is optional.
        /// </summary>
        public TimeSpan? TimePerDayPerCollaborator { get; set; }

        /// <summary>
        /// Gets or sets the total time tracked per month per Collaborator. This is optional.
        /// </summary>
        public TimeSpan? TimePerMonthPerCollaborator { get; set; }
    }
}
