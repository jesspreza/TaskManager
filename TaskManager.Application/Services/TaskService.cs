using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Services
{
    public class TaskService : BaseService<Task, Guid, TaskRequestDto, TaskResponseDto>, ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
            : base(taskRepository, mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<TaskListResponseDto>> SearchTasksAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IEnumerable<Task> filteredTasks;
            int totalCount;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredTasks = await _taskRepository.GetManyAsync(t =>
                (t.Name.Contains(searchTerm) ||
                t.Project.Name.Contains(searchTerm) ||
                t.TimeTrackers.Any(tt => tt.Collaborator.Name.Contains(searchTerm))) &&
                !t.DeletedAt.HasValue);
            }
            else
            {
                filteredTasks = await _taskRepository.GetManyAsync(t => !t.DeletedAt.HasValue);
            }

            totalCount = filteredTasks.Count();

            var pagedTasks = filteredTasks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var taskDtos = _mapper.Map<IEnumerable<TaskListResponseDto>>(pagedTasks);

            // Preencher os campos calculados
            foreach (var taskDto in taskDtos)
            {
                CalculateTimeTrackers(taskDto);
                CalculateTimeTrackersPerCollaborator(taskDto);
            }

            return new PagedList<TaskListResponseDto>(taskDtos.ToList(), totalCount, pageNumber, pageSize);
        }

        private void CalculateTimeTrackers(TaskListResponseDto taskDto)
        {
            var timeTrackers = taskDto.TimeTrackerReportResponseDto;

            if (timeTrackers == null || !timeTrackers.Any())
            {
                taskDto.TimePerDayPerTask = TimeSpan.Zero;
                taskDto.TimePerMonthPerTask = TimeSpan.Zero;
                return;
            }

            var now = DateTime.UtcNow;
            var today = now.Date;

            var currentDayTasks = timeTrackers
                .Where(t => t.StartDate < today.AddDays(1) && (t.EndDate == null || t.EndDate > today))
                .Select(t => new
                {
                    Start = t.StartDate < today ? today : t.StartDate,
                    End = t.EndDate.HasValue && t.EndDate < today.AddDays(1) ? t.EndDate.Value : now
                }).ToList();

            taskDto.TimePerDayPerTask = TimeSpan.FromMinutes(currentDayTasks.Sum(t => (t.End - t.Start).Value.TotalMinutes));

            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var currentMonthTasks = timeTrackers
                .Where(t => t.StartDate < today.AddDays(1) && (t.EndDate == null || t.EndDate > firstDayOfMonth))
                .Select(t => new
                {
                    Start = t.StartDate < firstDayOfMonth ? firstDayOfMonth : t.StartDate,
                    End = t.EndDate.HasValue && t.EndDate < today.AddDays(1) ? t.EndDate.Value : now
                }).ToList();

            taskDto.TimePerMonthPerTask = TimeSpan.FromMinutes(currentMonthTasks.Sum(t => (t.End - t.Start).Value.TotalMinutes));
        }

        private void CalculateTimeTrackersPerCollaborator(TaskListResponseDto taskDto)
        {
            var timeTrackers = taskDto.TimeTrackerReportResponseDto;

            if (timeTrackers == null || !timeTrackers.Any())
            {
                return;
            }

            var now = DateTime.UtcNow;

            // Para o cálculo do tempo por colaborador no dia atual
            var today = now.Date;
            var collaboratorsDayTime = timeTrackers
                .Where(t => t.StartDate < today.AddDays(1) && (t.EndDate == null || t.EndDate > today))
                .GroupBy(t => t.CollaboratorName)
                .Select(g => new
                {
                    CollaboratorName = g.Key,
                    TimeSpentPerDay = TimeSpan.FromMinutes(g.Sum(t =>
                    {
                        var start = t.StartDate < today ? today : t.StartDate;
                        var end = t.EndDate.HasValue && t.EndDate < today.AddDays(1) ? t.EndDate.Value : now;
                        return (end - start).Value.TotalMinutes;
                    }))
                }).ToList();

            // Para o cálculo do tempo por colaborador no mês corrente
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var collaboratorsMonthTime = timeTrackers
                .Where(t => t.StartDate < today.AddDays(1) && (t.EndDate == null || t.EndDate > firstDayOfMonth))
                .GroupBy(t => t.CollaboratorName)
                .Select(g => new
                {
                    CollaboratorName = g.Key,
                    TimeSpentPerMonth = TimeSpan.FromMinutes(g.Sum(t =>
                    {
                        var start = t.StartDate < firstDayOfMonth ? firstDayOfMonth : t.StartDate;
                        var end = t.EndDate.HasValue && t.EndDate < today.AddDays(1) ? t.EndDate.Value : now;
                        return (end - start).Value.TotalMinutes;
                    }))
                }).ToList();

            foreach (var tracker in timeTrackers)
            {
                var dayTime = collaboratorsDayTime.FirstOrDefault(c => c.CollaboratorName == tracker.CollaboratorName);
                var monthTime = collaboratorsMonthTime.FirstOrDefault(c => c.CollaboratorName == tracker.CollaboratorName);

                if (dayTime != null)
                {
                    tracker.TimePerDayPerCollaborator = dayTime.TimeSpentPerDay;
                }

                if (monthTime != null)
                {
                    tracker.TimePerMonthPerCollaborator = monthTime.TimeSpentPerMonth;
                }
            }
        }

        protected override void PreUpdate(Task entity, TaskRequestDto dto)
        {
            entity.UpdateDomain(dto.Name, dto.Description, dto.ProjectId);
        }
    }
}
