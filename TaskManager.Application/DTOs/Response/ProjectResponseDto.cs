namespace TaskManager.Application.DTOs.Response
{
    public class ProjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        //public ICollection<TaskResponseDto>? TasksResponseDto { get; set; }
    }
}
