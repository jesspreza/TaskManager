using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Application.DTOs.Response
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [NotMapped]
        public IEnumerator<TaskResponseDto>? TasksResponseDto { get; set; }
    }
}
