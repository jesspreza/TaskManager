namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing a Task in response operations.
    /// </summary>
    public class TaskResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Task.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the Task. This is optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the date when the Task was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the Task was last updated. This is optional.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date when the Task was deleted. This is optional.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the Project associated with the Task.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project details associated with the Task. This is optional.
        /// </summary>
        public ProjectResponseDto? ProjectResponseDto { get; set; }

        /// <summary>
        /// Gets or sets the collection of Time Tracker entries associated with the Task. This is optional.
        /// </summary>
        public ICollection<TimeTrackerResponseDto>? TimeTrackersResponseDto { get; set; }
    }
}
