namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing Project details in response operations.
    /// </summary>
    public class ProjectResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Project.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the Project.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the date when the Project was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the date when the Project was last updated.
        /// This is null if the Project has not been updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the date when the Project was deleted.
        /// This is null if the Project has not been deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
