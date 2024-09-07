using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    /// <summary>
    /// Data Transfer Object (DTO) for Task creation and update requests.
    /// </summary>
    public class TaskRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Task.
        /// This is optional during creation, but required for updates.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Task.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the Task.
        /// This field is optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated Project.
        /// This field is required.
        /// </summary>
        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectId { get; set; }
    }
}
