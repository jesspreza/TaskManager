using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object (DTO) for Collaborator creation and update requests.
    /// </summary>
    public class CollaboratorRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Collaborator.
        /// This is optional during creation, but required for updates.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Collaborator.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated User ID for the Collaborator.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "User is required")]
        public Guid UserId { get; set; }
    }
}
