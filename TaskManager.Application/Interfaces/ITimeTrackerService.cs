using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Pagination;

namespace TaskManager.Application.Interfaces
{
    public interface ITimeTrackerService : IBaseService<TimeTracker, Guid, TimeTrackerRequestDto, TimeTrackerResponseDto>
    {
        Task<PagedList<TimeTrackerResponseDto>> SearchTimeTrackersAsync(string? searchTerm, string? dateFilter, int pageNumber, int pageSize);
    }
}
