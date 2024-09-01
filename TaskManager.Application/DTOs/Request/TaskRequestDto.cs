using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs.Request
{
    public class TaskRequestDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectId { get; set; }
    }
}
