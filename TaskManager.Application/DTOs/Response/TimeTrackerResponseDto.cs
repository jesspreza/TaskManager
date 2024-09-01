namespace TaskManager.Application.DTOs.Response
{
    public class TimeTrackerResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeZoneId { get; set; }
        public Guid TaskId { get; set; }
        public virtual TaskResponseDto? TaskResponseDto { get; set; }
        public Guid? CollaboratorId { get; set; }
        public CollaboratorResponseDto? CollaboratorResponseDto { get; set; }
    }
}
