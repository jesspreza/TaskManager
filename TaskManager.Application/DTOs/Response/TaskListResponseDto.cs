namespace TaskManager.Application.DTOs.Response
{
    public class TaskListResponseDto : TimeTrackerReportResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TimeSpan? TimePerDayPerTask { get; set; }
        public TimeSpan? TimePerMonthPerTask { get; set; }

        public TimeTrackerReportResponseDto[]? TimeTrackerReportResponseDto { get; set; }
    }
    
}
