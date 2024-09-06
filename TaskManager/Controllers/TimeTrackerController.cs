using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Extensions;
using TaskManager.Api.Models;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Pagination;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TimeTrackerController : Controller
    {
        private readonly ITimeTrackerService _timeTrackerService;

        public TimeTrackerController(ITimeTrackerService timeTrackerService)
        {
            _timeTrackerService = timeTrackerService;
        }

        /// <summary>
        /// Get a list of time trackers with pagination.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of time trackers.</returns>
        /// <response code="200">Returns a paginated list of time trackers.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<TimeTrackerResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _timeTrackerService.GetAllPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);

                Response.AddPaginationHeader(new PaginationHeader(
                    result.CurrentPage,
                    result.PageSize,
                    result.TotalCount,
                    result.TotalPages
                    ));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Search for time trackers based on a search term and date filter with pagination.
        /// </summary>
        /// <param name="searchTerm">The term to search for within time tracker descriptions or other fields.</param>
        /// <param name="dateFilter">Optional date filter for time trackers.</param>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of time trackers that match the search term and date filter.</returns>
        /// <response code="200">Returns a paginated list of time trackers matching the search term and date filter.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedList<TimeTrackerResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchTimeTrackers([FromQuery] string? searchTerm, string? dateFilter, [FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _timeTrackerService.SearchTimeTrackersAsync(searchTerm, dateFilter, paginationParams.PageNumber, paginationParams.PageSize);

                Response.AddPaginationHeader(new PaginationHeader(
                   result.CurrentPage,
                   result.PageSize,
                   result.TotalCount,
                   result.TotalPages
               ));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get a time tracker by ID.
        /// </summary>
        /// <param name="id">ID of the time tracker.</param>
        /// <returns>The specified time tracker.</returns>
        /// <response code="200">Returns the specified time tracker.</response>
        /// <response code="404">If the time tracker with the specified ID is not found.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TimeTrackerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TimeTrackerResponseDto>> GetById(Guid id)
        {
            try
            {
                var result = await _timeTrackerService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new time tracker.
        /// </summary>
        /// <param name="TimeTrackerRequestDto">Details of the time tracker to create.</param>
        /// <returns>The created time tracker.</returns>
        /// <response code="201">Returns the created time tracker.</response>
        /// <response code="400">If the time tracker could not be created due to validation errors.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TimeTrackerResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TimeTrackerRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }
            try
            {
                var result = await _timeTrackerService.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Update an existing time tracker.
        /// </summary>
        /// <param name="id">ID of the time tracker to update.</param>
        /// <param name="TimeTrackerRequestDto">Updated details of the time tracker.</param>
        /// <returns>The updated time tracker.</returns>
        /// <response code="200">Returns the updated time tracker.</response>
        /// <response code="404">If the time tracker with the specified ID is not found.</response>
        /// <response code="400">If the time tracker could not be updated due to invalid data.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TimeTrackerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TimeTrackerRequestDto requestDto)
        {
            try
            {
                requestDto.Id = id;
                var result = await _timeTrackerService.UpdateAsync(id, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a time tracker by ID.
        /// </summary>
        /// <param name="id">ID of the time tracker to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the time tracker was deleted.</response>
        /// <response code="404">If the time tracker with the specified ID is not found.</response>
        /// <response code="400">If the deletion failed due to an invalid request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _timeTrackerService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"TimeTracker with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
