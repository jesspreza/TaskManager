using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Application.DTOs.Response
{
    public class TaskResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectResponseDto? ProjectResponseDto { get; set; }

        [NotMapped]
        public IEnumerable<TimeTrackerResponseDto>? TimeTrackersResponseDto { get; set; }
    }
}
