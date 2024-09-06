using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Application.Services
{
    public class TimeTrackerService : BaseService<TimeTracker, Guid, TimeTrackerRequestDto, TimeTrackerResponseDto>, ITimeTrackerService
    {
        private readonly ITimeTrackerRepository _timeTrackerRepository;
        private readonly IMapper _mapper;

        public TimeTrackerService(ITimeTrackerRepository timeTrackerRepository, IMapper mapper) : base(timeTrackerRepository, mapper)
        {
            _timeTrackerRepository = timeTrackerRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<TimeTrackerResponseDto>> SearchTimeTrackersAsync(string? searchTerm, string? dateFilter, int pageNumber, int pageSize)
        {
            IEnumerable<TimeTracker> filteredTimeTracker;
            int totalCount;
            List<TimeTracker>? pagedTimeTrackers;

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (!string.IsNullOrEmpty(dateFilter))
            {
                if (dateFilter == "today")
                {
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                }
                else if (dateFilter == "thisMonth")
                {
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                }
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredTimeTracker = await _timeTrackerRepository.GetManyAsync(t =>
                (t.Task.Name.Contains(searchTerm) ||
                t.Task.Project.Name.Contains(searchTerm) ||
                t.Collaborator.Name.Contains(searchTerm)) &&
                !t.DeletedAt.HasValue &&
                t.StartDate >= startDate && t.StartDate <= endDate);
            }
            else
            {
                filteredTimeTracker = await _timeTrackerRepository.GetManyAsync(t => 
                !t.DeletedAt.HasValue &&
                t.StartDate >= startDate && t.StartDate <= endDate);
            }

            totalCount = filteredTimeTracker.Count();

            pagedTimeTrackers = filteredTimeTracker.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var timeDtos = _mapper.Map<IEnumerable<TimeTrackerResponseDto>>(pagedTimeTrackers);

            return new PagedList<TimeTrackerResponseDto>(timeDtos.ToList(), totalCount, pageNumber, pageSize);
        }

        private async Task ValidateNoOverlapAsync(DateTime startDate, DateTime endDate, Guid taskId, Guid collaboratorId, Guid? existingId = null)
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

        private async Task ValidateTotalHoursPerDayAsync(DateTime startDate, DateTime endDate, Guid collaboratorId, Guid? existingId = null)
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
