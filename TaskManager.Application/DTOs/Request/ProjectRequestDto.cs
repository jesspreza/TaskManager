using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object (DTO) for Project creation and update requests.
    /// </summary>
    public class ProjectRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Project.
        /// This is optional during creation, but required for updates.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Project.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
