using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    public class ProjectDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public IEnumerator<TaskDto?> TasksDto { get; set; }
    }
}
