namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing Collaborator details in response operations.
    /// </summary>
    public class CollaboratorResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Collaborator.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the Collaborator.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the date when the Collaborator was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        // <summary>
        /// Gets or sets the date when the Collaborator was last updated.
        /// This is null if the Collaborator has not been updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the date when the Collaborator was deleted.
        /// This is null if the Collaborator has not been deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier of the User associated with the Collaborator.
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the associated User details as a UserResponseDto.
        /// This is null if the User data is not included in the response.
        /// </summary>
        public UserResponseDto? UserResponseDto { get; set; }

    }
}
