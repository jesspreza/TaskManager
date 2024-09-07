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
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get a list of tasks with pagination.
        /// </summary>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of tasks.</returns>
        /// <response code="200">Returns a paginated list of tasks.</response>
        /// <response code="400">If the pagination parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<TaskResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _taskService.GetAllPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);

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
        /// Search for tasks based on a search term with pagination.
        /// </summary>
        /// <param name="searchTerm">The term to search for within task names or descriptions.</param>
        /// <param name="paginationParams">Pagination parameters including page number and page size.</param>
        /// <returns>A paginated list of tasks that match the search term.</returns>
        /// <response code="200">Returns a paginated list of tasks matching the search term.</response>
        /// <response code="400">If the search or pagination parameters are invalid.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedList<TaskListResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchTasks([FromQuery] string? searchTerm, [FromQuery] PaginationParams paginationParams)
        {
            try
            {
                var result = await _taskService.SearchTasksAsync(searchTerm, paginationParams.PageNumber, paginationParams.PageSize);

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
                return BadRequest($"Could not find {ex.Message}");
            }
        }

        /// <summary>
        /// Get a task by ID.
        /// </summary>
        /// <param name="id">ID of the task.</param>
        /// <returns>The specified task.</returns>
        /// <response code="200">Returns the specified task.</response>
        /// <response code="404">If the task with the specified ID is not found.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskResponseDto>> GetById(Guid id)
        {
            try
            {
                var result = await _taskService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="requestDto">Details of the task to create.</param>
        /// <returns>The created task.</returns>
        /// <response code="201">Returns the created task.</response>
        /// <response code="400">If the task could not be created due to invalid data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TaskRequestDto requestDto)
        {
            try
            {
                var result = await _taskService.CreateAsync(requestDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="id">ID of the task to update.</param>
        /// <param name="requestDto">Updated details of the task.</param>
        /// <returns>The updated task.</returns>
        /// <response code="200">Returns the updated task.</response>
        /// <response code="404">If the task with the specified ID is not found.</response>
        /// <response code="400">If the task could not be updated due to invalid data.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] TaskRequestDto requestDto)
        {
            try
            {
                requestDto.Id = id;
                var result = await _taskService.UpdateAsync(id, requestDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a task by ID.
        /// </summary>
        /// <param name="id">ID of the task to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <response code="200">Returns a success message if the task was deleted.</response>
        /// <response code="404">If the task with the specified ID is not found.</response>
        /// <response code="400">If the deletion failed due to an invalid request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _taskService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with id {id} not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
