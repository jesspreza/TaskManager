namespace TaskManager.Application.DTOs.Response
{
    public class TimeTrackerReportResponseDto
    {
        public Guid TimeTrackerId { get; set; }
        public Guid TaskId { get; set; }
        public Guid CollaboratorId { get; set; }
        public string? CollaboratorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? TimePerDayPerCollaborator { get; set; }
        public TimeSpan? TimePerMonthPerCollaborator { get; set; }
    }
}
