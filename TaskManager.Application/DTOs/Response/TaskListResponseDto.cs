namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing a Task and its associated Time Tracker details in response operations.
    /// </summary>
    public class TaskListResponseDto : TimeTrackerReportResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Task.
        /// </summary>
        public Guid Id { get; set; }
        
        // <summary>
        /// Gets or sets the name of the Task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the Task. This is optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the Project associated with the Task.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name of the Project associated with the Task.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the date when the Task was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the Task was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the total time spent on the Task per day. This is optional.
        /// </summary>
        public TimeSpan? TimePerDayPerTask { get; set; }

        /// <summary>
        /// Gets or sets the total time spent on the Task per month. This is optional.
        /// </summary>
        public TimeSpan? TimePerMonthPerTask { get; set; }

        /// <summary>
        /// Gets or sets the list of Time Tracker reports associated with the Task.
        /// This is optional and may be null.
        /// </summary>
        public TimeTrackerReportResponseDto[]? TimeTrackerReportResponseDto { get; set; }
    }
    
}
