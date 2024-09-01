using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services
{
    public class TimeTrackerService : BaseService<TimeTracker, Guid, TimeTrackerRequestDto, TimeTrackerResponseDto>, ITimeTrackerService
    {
        private readonly ITimeTrackerRepository _timeTrackerRepository;

        public TimeTrackerService(ITimeTrackerRepository timeTrackerRepository, IMapper mapper) : base(timeTrackerRepository, mapper)
        {
            _timeTrackerRepository = timeTrackerRepository;
        }

        private async Task ValidateNoOverlapAsync(DateTime startDate, DateTime endDate, Guid taskId, Guid? collaboratorId, Guid? existingId = null)
        {
            var existingTrackers = await _timeTrackerRepository.GetManyAsync(tt => 
                tt.TaskId == taskId &&
                tt.CollaboratorId == collaboratorId && 
               !tt.DeletedAt.HasValue);

            foreach (var tracker in existingTrackers)
            {
                if (tracker.Id == existingId)
                    continue;

                if (startDate < tracker.EndDate && endDate > tracker.StartDate)
                {
                    throw new InvalidOperationException("The time interval overlaps with an existing time tracker.");
                }
            }

        }

        private async Task ValidateTotalHoursPerDayAsync(DateTime startDate, DateTime endDate, Guid? collaboratorId, Guid? existingId = null)
        {
            var startOfDay = startDate.Date;
            var endOfDay = startOfDay.AddDays(1);

            var existingTrackers = await _timeTrackerRepository.GetManyAsync(tt =>
                tt.CollaboratorId == collaboratorId &&
                !tt.DeletedAt.HasValue &&
                tt.StartDate >= startOfDay && tt.EndDate <= endOfDay);

            double totalHours = (endDate - startDate).TotalHours;

            foreach (var tracker in existingTrackers)
            {
                if (tracker.Id != existingId)
                {
                    totalHours += (tracker.EndDate - tracker.StartDate).TotalHours;
                }
            }

            if (totalHours > 24)
            {
                throw new InvalidOperationException("Total hours for this collaborator in the given day exceeds 24 hours.");
            }
        }

        protected override void PreUpdate(TimeTracker entity, TimeTrackerRequestDto dto)
        {
            ValidateNoOverlapAsync(dto.StartDate, dto.EndDate, dto.TaskId, dto.CollaboratorId, dto.Id).GetAwaiter().GetResult();
            ValidateTotalHoursPerDayAsync(dto.StartDate, dto.EndDate, dto.CollaboratorId, dto.Id).GetAwaiter().GetResult();

            entity.UpdateDomain(dto.StartDate, dto.EndDate, dto.TimeZoneId, dto.TaskId, dto.CollaboratorId);
        }

        protected override void PreCreate(TimeTracker entity, TimeTrackerRequestDto dto)
        {
            ValidateNoOverlapAsync(dto.StartDate, dto.EndDate, dto.TaskId, dto.CollaboratorId, dto.Id).GetAwaiter().GetResult();
            ValidateTotalHoursPerDayAsync(dto.StartDate, dto.EndDate, dto.CollaboratorId, dto.Id).GetAwaiter().GetResult();
        }
    }
}
