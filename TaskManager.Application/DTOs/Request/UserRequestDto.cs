using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object (DTO) for User creation and update requests.
    /// </summary>
    public class UserRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the User.
        /// This is optional during creation, but required for updates.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the username for the User.
        /// This field is required and must be between 3 and 250 characters.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(250, ErrorMessage = "Username must be less than 250 characters")]
        [MinLength(3, ErrorMessage = "Username must be at leas 3 characters long")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the User.
        /// This field is required and must be between 6 and 512 characters.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(512, ErrorMessage = "Password must be shorter")]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters long")]
        public string Password { get; set; }
    }
}
