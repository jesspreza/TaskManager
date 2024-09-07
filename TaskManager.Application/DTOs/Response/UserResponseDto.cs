namespace TaskManager.Application.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object for representing a User in response operations.
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the User.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the User.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the User was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the User was last updated. This is optional.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the User was deleted. This is optional.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
