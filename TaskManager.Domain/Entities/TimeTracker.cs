using TaskManager.Domain.Validations;

namespace TaskManager.Domain.Entities
{
    public partial class TimeTracker
    {
        public TimeTracker(DateTime startDate, DateTime endDate, string timeZoneId, Guid taskId, Guid? collaboratorId) : base()
        {
            ValidateDomain(startDate, endDate, taskId, timeZoneId);
            StartDate = startDate;
            EndDate = endDate;
            TimeZoneId = timeZoneId;
            TaskId = taskId;
            CollaboratorId = collaboratorId;
        }

        public void UpdateDomain(DateTime startDate, DateTime endDate, string timeZoneId, Guid taskId, Guid? collaboratorId)
        {
            ValidateDomain(startDate, endDate, taskId, timeZoneId);
            StartDate = startDate;
            EndDate = endDate;
            TimeZoneId = timeZoneId;
            TaskId = taskId;
            CollaboratorId = collaboratorId;
            Update();
        }

        public static void ValidateDomain(DateTime startDate, DateTime endDate, Guid taskId, string timeZoneId)
        {
            DomainExceptionValidation.When(startDate > endDate, "Start date cannot be later than end date");
            DomainExceptionValidation.When(taskId == Guid.Empty, "Task id must be provided");
            DomainExceptionValidation.When(string.IsNullOrEmpty(timeZoneId), "Time zone must be provided");
        }
    }
}
