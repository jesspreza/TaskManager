namespace TaskManager.Application.DTOs.Response
{
    public class CollaboratorResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid UserId { get; set; }
        public UserResponseDto? UserResponseDto { get; set; }

        //public ICollection<TimeTrackerResponseDto>? TimeTrackersResponseDto { get; set; }
    }
}
